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
    public class LocalizedLineDiscover : LocalizedLine
    {
        public string TextInLearningLang = "";
    }

    public class DiscoverLineProvider : LineProviderBehaviour
    {
        [SerializeField]
        internal LocalizedStringTable? stringsTable;

        [SerializeField]
        internal LocalizedAssetTable? assetTable;

        private LineParser lineParser = new LineParser();

        private BuiltInMarkupReplacer builtInReplacer = new BuiltInMarkupReplacer();


        public void SetStringTable(LocalizedStringTable table)
        {
            stringsTable = table;
        }

        public override string LocaleCode
        {
            get
            {
                return LocalizationSettings.SelectedLocale.Identifier.Code;
            }
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

            LocalizedDatabase<UnityEngine.Localization.Tables.StringTable, UnityEngine.Localization.Tables.StringTableEntry>.TableEntryResult tableEntryResult = await YarnTask.WaitForAsyncOperation(LocalizationSettings.StringDatabase.GetTableEntryAsync(stringsTable.TableReference, line.ID, null, FallbackBehavior.UseFallback), cancellationToken);
            Yarn.Unity.UnityLocalization.LineMetadata? metadata = tableEntryResult.Entry?.SharedEntry.Metadata.GetMetadata<Yarn.Unity.UnityLocalization.LineMetadata>();
            string text = tableEntryResult.Entry?.LocalizedValue ?? $"!! Error: Missing localisation for line {line.ID} in string table {tableEntryResult.Table.LocaleIdentifier}";
            string? shadowLineID = metadata?.ShadowLineSource;
            if (shadowLineID != null)
            {
                LocalizedDatabase<UnityEngine.Localization.Tables.StringTable, UnityEngine.Localization.Tables.StringTableEntry>.TableEntryResult tableEntryResult2 = await YarnTask.WaitForAsyncOperation(LocalizationSettings.StringDatabase.GetTableEntryAsync(stringsTable.TableReference, shadowLineID, null, FallbackBehavior.UseFallback), cancellationToken);
                if (tableEntryResult2.Entry == null)
                {
                    Debug.LogWarning($"Line {line.ID} shadows line {shadowLineID}, but no such entry was found in the string table {stringsTable.TableReference}");
                }
                else
                {
                    text = tableEntryResult2.Entry.LocalizedValue;
                }
            }

            MarkupParseResult markup = lineParser.ParseString(LineParser.ExpandSubstitutions(text, line.Substitutions), LocaleCode);
            UnityEngine.Object? asset = null;
            if (assetTable != null && !assetTable.IsEmpty)
            {
                asset = await YarnTask.WaitForAsyncOperation(LocalizationSettings.AssetDatabase.GetLocalizedAssetAsync<UnityEngine.Object>(assetTable.TableReference, shadowLineID ?? line.ID, null, FallbackBehavior.UseFallback), cancellationToken);
            }
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

            return new LocalizedLineDiscover
            {
                Text = markup,
                TextInLearningLang = TextInLearningLanguage, // this is our custom added line!
                TextID = line.ID,
                Substitutions = line.Substitutions,
                RawText = text,
                Metadata = (metadata?.tags ?? Array.Empty<string>()),
                Asset = asset
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

