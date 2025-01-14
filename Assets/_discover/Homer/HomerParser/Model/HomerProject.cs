using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Homer
{
    public class HomerProject
    {
        public string _id;
        public Int64 _date;
        public string _name;
        public string _version;
        public string _locale;

        public string _apiVersion;
        
        public HomerLocale _mainLocale;
        public HomerLocale[] _availableLocale;
        public HomerFlow[] _flows;

        public HomerActor[] _actors;
        public HomerVariable[] _variables;
        public HomerMetadata[] _metadata;

        public HomerLabel[] _labels;

        public HomerFlow GetFlow(string name)
        {
            return null;
        }

        public HomerConnection GetConnectionsByElementId(HomerNode node, string nodeElementId)
        {
            HomerConnection c = null;

            foreach (HomerConnection connection in node._connections)
            {
                if (connection._nodeElementId == nodeElementId)
                    c = connection;
            }
            return c;
        }
        
        public HomerMetadata GetMetadataByName(string name) {
            HomerMetadata metadata = null;
            foreach (var meta in _metadata)
            {
                if (meta._name == name)
                    metadata = meta;
            }
            return metadata;
        }

        public HomerMetadata GetMetadataById(string id) {
            HomerMetadata metadata = null;
            foreach (var meta in _metadata)
            {
                if (meta._id == id)
                    metadata = meta;
            }
            return metadata;
        }
        
        public HomerMetadata GetMetadataByUID(string uid) {
            HomerMetadata metadata = null;
            foreach (var meta in _metadata)
            {
                if (meta._uid == uid)
                    metadata = meta;
            }
            return metadata;
        }
        
        public HomerMetadataValue GetMetadataValueById(string metadataValueId) {
            HomerMetadataValue metadatav = null;
            foreach (var meta in _metadata)
            {
                foreach (var metaValue in meta._values)
                {
                    if (metaValue._id == metadataValueId)
                    {
                        metadatav = metaValue;
                        break;
                    }
                }
            }
            return metadatav;
        }
        
        public HomerMetadata GetMetadataByValueId(string metadataValueId) {
            HomerMetadata metadata = null;
            foreach (var meta in _metadata)
            {
                foreach (var metaValue in meta._values)
                {
                    if (metaValue._id == metadataValueId)
                    {
                        metadata = GetMetadataById(metaValue._metadataId);
                        break;
                    }
                }
            }
            return metadata;
        }
        
    }
}
