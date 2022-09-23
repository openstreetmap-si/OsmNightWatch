﻿using OsmNightWatch.Lib;
using OsmSharp;
using OsmSharp.Changesets;
using OsmSharp.Db;

namespace OsmNightWatch.Analyzers
{
    public interface IOsmAnalyzer
    {
        string AnalyzerName { get; }

        FilterSettings FilterSettings { get; }

        IEnumerable<IssueData> AnalyzeChanges(OsmChange changeset, IOsmGeoSource oldOsmSource, IOsmGeoSource newOsmSource);

        IEnumerable<IssueData> Initialize(IEnumerable<OsmGeo> relevatThings, IOsmGeoSource oldOsmSource, IOsmGeoSource newOsmSource);
    }
}
