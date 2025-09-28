#nullable enable
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using Yarn;
using Yarn.Markup;
using Yarn.Unity;
using Yarn.Compiler;
using Yarn.Unity.UnityLocalization;

namespace Antura.Discover
{

    /// <summary>
    /// Contains Yarn Spinner related metadata for Unity string table entries.
    /// </summary>
    public class LineMetadata : UnityEngine.Localization.Metadata.IMetadata
    {
        /// <summary>
        /// The name of the Yarn node that this line came from.
        /// </summary>
        public string nodeName = "";

        /// <summary>
        /// The <c>#hashtags</c> present on the line.
        /// </summary>
        public string[] tags = System.Array.Empty<string>();

        /// <summary>
        /// Gets the line ID indicated by any shadow tag contained in this
        /// metadata, if present.
        /// </summary>
        public string? ShadowLineSource
        {
            get
            {
                foreach (var metadataEntry in tags)
                {
                    if (metadataEntry.StartsWith("shadow:") != false)
                    {
                        // This is a shadow line. Return the line ID that it's
                        // shadowing.
                        return "line:" + metadataEntry.Substring("shadow:".Length);
                    }
                }

                // The line had metadata, but it wasn't a shadow line.
                return null;
            }
        }
    }

    public class LocalizedLineDiscover : LocalizedLine
    {
        public string TextInLearningLang = "";
        public UnityEngine.Object? AssetInLearningLang = null;
    }

    public class DiscoverLineProvider : LineProviderBehaviour
    {
        [SerializeField]
        internal LocalizedStringTable? stringsTable;

        [SerializeField]
        internal LocalizedAssetTable? assetTable;

        private LineParser lineParser = new LineParser();

        private BuiltInMarkupReplacer builtInReplacer = new BuiltInMarkupReplacer();

        // Warn once per key when an asset lookup returns null in non-editor players
        private readonly HashSet<string> _assetNullWarnedKeys = new HashSet<string>();

        public void SetStringTable(LocalizedStringTable table)
        {
            stringsTable = table;
        }

        public void SetAssetTable(LocalizedAssetTable table)
        {
            assetTable = table;
        }

        public override string LocaleCode
        {
            get => LocalizationSettings.SelectedLocale.Identifier.Code;
            set
            {
                Locale locale = LocalizationSettings.AvailableLocales.GetLocale(value);
                if (locale == null)
                {
                    throw new InvalidOperationException("Can't set locale to " + value + ": no such locale has been configured");
                }

                LocalizationSettings.SelectedLocale = locale;
            }
        }

        private void Awake()
        {
            lineParser.RegisterMarkerProcessor("select", builtInReplacer);
            lineParser.RegisterMarkerProcessor("plural", builtInReplacer);
            lineParser.RegisterMarkerProcessor("ordinal", builtInReplacer);
        }

        public override async YarnTask<LocalizedLine> GetLocalizedLineAsync(Line line, CancellationToken cancellationToken)
        {
            if (stringsTable == null || stringsTable.IsEmpty)
            {
                throw new InvalidOperationException("Tried to get localised line for " + line.ID + ", but no string table has been set.");
            }

            var getStringOp = LocalizationSettings.StringDatabase.GetTableEntryAsync(stringsTable.TableReference, line.ID, null, FallbackBehavior.UseFallback);
            var entry = await YarnTask.WaitForAsyncOperation(getStringOp, cancellationToken);

            // Attempt to fetch metadata tags for this line from the string table
            var metadata = entry.Entry?.SharedEntry.Metadata.GetMetadata<LineMetadata>();

            // Get the text from the entry
            var text = entry.Entry?.LocalizedValue
                ?? $"!! Error: Missing localisation for line {line.ID} in string table {entry.Table.LocaleIdentifier}";

            string? shadowLineID = metadata?.ShadowLineSource;

            if (shadowLineID != null)
            {
                // This line actually shadows another line. Fetch that line, and
                // use its text (but not its metadata)
                var getShadowLineOp = LocalizationSettings.StringDatabase.GetTableEntryAsync(stringsTable.TableReference, shadowLineID, null, FallbackBehavior.UseFallback);
                var shadowEntry = await YarnTask.WaitForAsyncOperation(getShadowLineOp, cancellationToken);
                if (shadowEntry.Entry == null)
                {
                    Debug.LogWarning($"Line {line.ID} shadows line {shadowLineID}, but no such entry was found in the string table {stringsTable.TableReference}");
                }
                else
                {
                    text = shadowEntry.Entry.LocalizedValue;
                }
            }

            // We now have our text; parse it as markup
            var markup = lineParser.ParseString(LineParser.ExpandSubstitutions(text, line.Substitutions), this.LocaleCode);

            // Fetch the same entry in learning language
            string TextInLearningLanguage = string.Empty;
            try
            {
                TextInLearningLanguage = await DiscoverAppManager.I.GetLearningLocalizedStringAsync(
                    stringsTable.TableReference,
                    line.ID,
                    shadowLineID,
                    FallbackBehavior.UseFallback,
                    cancellationToken
                );
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[DiscoverLineProvider] Learning language fetch failed for line {line.ID}: {ex.Message}");
            }


            // Lastly, attempt to fetch an asset for this line
            UnityEngine.Object? asset = null;
            UnityEngine.Object? assetLearning = null;

            if (assetTable != null && assetTable.IsEmpty == false)
            {
                var loadOp = LocalizationSettings.AssetDatabase.GetLocalizedAssetAsync<UnityEngine.Object>(assetTable.TableReference, shadowLineID ?? line.ID, null, FallbackBehavior.UseFallback);
                asset = await YarnTask.WaitForAsyncOperation(loadOp, cancellationToken);

                var loadOpLearning = LocalizationSettings.AssetDatabase.GetLocalizedAssetAsync<UnityEngine.Object>(assetTable.TableReference, shadowLineID ?? line.ID, DiscoverAppManager.I.GetLearningLocale(), FallbackBehavior.UseFallback);
                assetLearning = await YarnTask.WaitForAsyncOperation(loadOpLearning, cancellationToken);
            }

            // Debug.Log($"[DiscoverLineProvider] Fetched line {line.ID} (shadowing {shadowLineID ?? "none"}), text: {text}, asset: {(asset != null ? asset.name : "none")}, asset: {(assetLearning != null ? assetLearning.name : "none")}");

            return new LocalizedLineDiscover
            {
                Text = markup,
                TextInLearningLang = TextInLearningLanguage, // ANTURA this is our custom added line!
                TextID = line.ID,
                Substitutions = line.Substitutions,
                RawText = text,
                Metadata = metadata?.tags ?? Array.Empty<string>(),
                Asset = asset,
                AssetInLearningLang = assetLearning  // ANTURA this is our custom added line!
            };
        }

        public override void RegisterMarkerProcessor(string attributeName, IAttributeMarkerProcessor markerProcessor)
        {
            lineParser.RegisterMarkerProcessor(attributeName, markerProcessor);
        }

        public override void DeregisterMarkerProcessor(string attributeName)
        {
            lineParser.DeregisterMarkerProcessor(attributeName);
        }

        public override YarnTask PrepareForLinesAsync(IEnumerable<string> lineIDs, CancellationToken cancellationToken)
        {
            return YarnTask.CompletedTask;
        }
    }
}

