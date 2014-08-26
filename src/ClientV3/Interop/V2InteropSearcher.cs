﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JsonLD.Core;
using Newtonsoft.Json.Linq;

namespace NuGet.VisualStudio.ClientV3.Interop
{
    public class V2InteropSearcher : IPackageSearcher
    {
        private IPackageRepository _repository;

        public V2InteropSearcher(IPackageRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<JToken>> Search(string searchTerm, SearchFilter filters, int skip, int take, CancellationToken ct)
        {
            return Task.Factory.StartNew(() => _repository.Search(
                searchTerm, filters.SupportedFrameworks.Select(fx => fx.FullName), filters.IncludePrerelease)
                .Skip(skip)
                .Take(take)
                .ToList()
                .Select(p => CreatePackageSearchResult(p)), ct);
        }
        private JToken CreatePackageSearchResult(IPackage package)
        {
            // Need to fetch all the versions of this package (this is slow, but we're in V2-interop land, so whatever :))
            var versions = _repository.FindPackagesById(package.Id);

            return PackageJsonLd.CreatePackageSearchResult(package, versions);
        }
    }
}