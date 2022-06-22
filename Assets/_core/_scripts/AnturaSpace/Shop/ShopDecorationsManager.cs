using Antura.AnturaSpace.UI;
using Antura.Core;
using Antura.Utilities;
using DG.DeInspektor.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Antura.AnturaSpace
{
    public class ShopDecorationsManager : SingletonMonoBehaviour<ShopDecorationsManager>
    {

        public Transform deletePropButtonTransform;
        public GameObject slotFeedbackPrefabGo;
        public float thresholdForDelete = 200;
        public float thresholdForPlacement = 200;

        private List<ShopDecorationObject> allShopDecorations = new List<ShopDecorationObject>();
        private List<ShopDecorationSlot> allShopDecorationSlots = new List<ShopDecorationSlot>();
        private ShopState shopState;
        private ShopContext shopContext;

        public List<ShopDecorationSlot> GetDecorationSlots()
        {
            return allShopDecorationSlots;
        }

        [Header("Debug")]
        public bool testDecorationFilling = false;

        public Action<ShopContext> OnContextChange;
        public Action OnPurchaseConfirmationRequested;
        public Action OnDeleteConfirmationRequested;
        public Action OnPurchaseComplete;
        public Action OnPurchaseCancelled;

#if UNITY_EDITOR
        // @note: call this to setup slots after you change them
        [DeMethodButton("Update Slot Indexes")]
        public void EditorSetup()
        {
            foreach (var slotGroup in GetComponentsInChildren<ShopDecorationSlotGroup>())
            {
                slotGroup.UpdateSlotIndexes();
            }
        }
#endif

        public bool HasSlotsForDecoration(ShopDecorationObject decorationObjectToTest)
        {
            bool result = allShopDecorationSlots.Count(x => x.IsFreeAndAssignableTo(decorationObjectToTest)) > 0;
            //Debug.Log("Has slots? " + result);
            return result;
        }

        #region Context

        public ShopContext ShopContext { get { return shopContext; } }
        private ShopContext previousShopContext;

        public void SetPreviousContext()
        {
            shopContext = previousShopContext;
            if (OnContextChange != null)
            { OnContextChange(shopContext); }
            AnturaSpaceUI.I.ToggledShopUI.gameObject.SetActive(shopContext == ShopContext.Purchase);
        }

        private void SwitchToContext(ShopContext newContext)
        {
            //Debug.Log("SWITCH TO CONTEXT: " + newContext);
            previousShopContext = shopContext;
            shopContext = newContext;
            if (OnContextChange != null)
            { OnContextChange(shopContext); }
            AnturaSpaceUI.I.ToggledShopUI.gameObject.SetActive(shopContext == ShopContext.Purchase);
        }

        public void SetContextCustomization()
        {
            SwitchToContext(ShopContext.Customization);
        }

        public void SetContextClosed()
        {
            SwitchToContext(ShopContext.Closed);
        }

        public void SetContextSpecialAction()
        {
            SwitchToContext(ShopContext.SpecialAction);
        }

        public void SetContextNewPlacement()
        {
            SwitchToContext(ShopContext.NewPlacement);
        }

        private void SetContextMovingPlacement()
        {
            SwitchToContext(ShopContext.MovingPlacement);
        }

        public void SetContextPurchase()
        {
            EndPlacementContext();
            ResetObjectHighlights();

            SwitchToContext(ShopContext.Purchase);
        }

        public void SetContextHidden()
        {
            SwitchToContext(ShopContext.Hidden);
        }

        #endregion


        #region Initialisation

        public void Initialise(ShopState shopState)
        {
            this.shopState = shopState;

            allShopDecorations = new List<ShopDecorationObject>(GetComponentsInChildren<ShopDecorationObject>());
            foreach (var shopDecoration in allShopDecorations)
            {
                shopDecoration.gameObject.SetActive(false);
            }
            //Debug.Log("Decorations: " + allShopDecorations.Count);

            allShopDecorationSlots = new List<ShopDecorationSlot>(GetComponentsInChildren<ShopDecorationSlot>());
            foreach (var slot in allShopDecorationSlots)
            {
                slot.Initialise(slotFeedbackPrefabGo);
            }
            //Debug.Log("Slots: " + allShopDecorationSlots.Count);

            // Load state
            if (shopState.occupiedSlots != null)
            {
                for (int i = 0; i < shopState.occupiedSlots.Length; i++)
                {
                    var slotState = shopState.occupiedSlots[i];
                    if (slotState.decorationID == "")
                    { continue; }
                    var decorationPrefab = allShopDecorations.Find(x => x.id == slotState.decorationID);
                    var slot = allShopDecorationSlots.FirstOrDefault(x => x.slotType == decorationPrefab.slotType && x.slotIndex == slotState.slotIndex);
                    if (slot != null && decorationPrefab != null)
                    {
                        //Debug.Log("LOADING ASSIGNED " + slot + " AND " + decorationPrefab);
                        var newDeco = SpawnNewDecoration(decorationPrefab);
                        slot.Assign(newDeco);
                    }
                }
            }

            //Debug.Log(shopState.ToString());

            // Initialise context
            SetContextClosed();

            // TEST
            if (testDecorationFilling)
            {
                var allPrefabDecorations = FindObjectsOfType<ShopAction_UnlockDecoration>().ToList().ConvertAll(x => x.UnlockableDecorationObject).ToList();
                foreach (var slot in allShopDecorationSlots)
                {
                    var prefab = allPrefabDecorations.FirstOrDefault(x => x.slotType == slot.slotType);
                    if (prefab != null)
                    {
                        slot.Assign(Instantiate(prefab));
                    }
                }
            }
        }


        private ShopDecorationSlot preDeleteSlot;
        private bool hoveringOnDeleteButton;
        public void OnEnterDeleteButton()
        {
            hoveringOnDeleteButton = true;
        }
        public void OnExitDeleteButton()
        {
            hoveringOnDeleteButton = false;
        }

        #endregion

        private ShopDecorationObject SpawnNewDecoration(ShopDecorationObject UnlockableDecorationPrefab)
        {
            if (!HasSlotsForDecoration(UnlockableDecorationPrefab))
            { return null; }

            var newDecoration = Instantiate(UnlockableDecorationPrefab);
            newDecoration.transform.localPosition = new Vector3(10000, 0, 0);
            newDecoration.Initialise(slotFeedbackPrefabGo);
            return newDecoration;
        }

        #region Deletion

        private void DeleteDecoration(ShopDecorationObject decoToDelete)
        {
            var assignedSlot = allShopDecorationSlots.FirstOrDefault(x => x.HasCurrentlyAssigned(currentDraggedDecoration));
            if (assignedSlot != null)
            {
                assignedSlot.Free();
            }
            Destroy(decoToDelete.gameObject);
        }

        public void DeleteAllDecorations()
        {
            foreach (var slot in allShopDecorationSlots)
            {
                if (slot.Assigned)
                {
                    var decoToDelete = slot.AssignedDecorationObject;
                    slot.Free();
                    Destroy(decoToDelete.gameObject);
                }
            }
            SaveState();
        }

        #endregion

        #region Drag Placement

        private Coroutine dragCoroutine;
        private ShopDecorationObject currentDraggedDecoration;
        private ShopDecorationSlot currentDraggedSlot;
        private ShopDecorationSlot startDragSlot;
        [HideInInspector]
        public int CurrentDecorationCost = 0;

        public Action<ShopDecorationObject> OnDragStart;
        public Action OnDragStop;

        public void CreateAndStartDragPlacement(ShopDecorationObject prefab, int bonesCost)
        {
            CurrentDecorationCost = bonesCost;
            var newDeco = SpawnNewDecoration(prefab);
            StartDragPlacement(newDeco, true);
        }

        public void StartDragPlacement(ShopDecorationObject decoToDrag, bool isNew)
        {
            if (isNew)
            {
                SetContextNewPlacement();
            }
            else
            {
                SetContextMovingPlacement();
            }

            currentDraggedSlot = null;
            currentDraggedDecoration = decoToDrag;
            currentDraggedDecoration.FocusHighlight(true);
            dragCoroutine = StartCoroutine(DragPlacementCO());

            if (OnDragStart != null)
            { OnDragStart(decoToDrag); }
        }

        private void StopDragPlacement()
        {
            if (OnDragStop != null)
            { OnDragStop(); }
            //currentDraggedDecoration.FocusHighlight(false);
            StopCoroutine(dragCoroutine);
        }

        private void EndPlacementContext()
        {
            ResetSlotHighlights();
            currentDraggedDecoration = null;
            currentDraggedSlot = null;
            startDragSlot = null;
        }

        private IEnumerator DragPlacementCO()
        {
            while (true)
            {
                // Get the closest assignable slot
                var allAssignableSlots = allShopDecorationSlots.Where(x =>
                    x.IsFreeAndAssignableTo(currentDraggedDecoration) || x.HasCurrentlyAssigned(currentDraggedDecoration));
                ShopDecorationSlot closestSlot = null;
                float minDistance = Int32.MaxValue;
                foreach (var slot in allAssignableSlots)
                {
                    var mousePos = AnturaSpaceUI.I.ScreenToUIPoint(Input.mousePosition);
                    var slotPos = AnturaSpaceUI.I.WorldToUIPoint(slot.transform.position);
                    float distance = (mousePos - slotPos).sqrMagnitude;

                    if (distance < minDistance && distance < thresholdForPlacement * thresholdForPlacement)
                    {
                        minDistance = distance;
                        closestSlot = slot;
                    }
                }

                // Check whether we are close to the delete button, instead
                bool shouldBeDeleted = false;
                if (hoveringOnDeleteButton)
                {
                    // First time: feedback
                    if (currentDraggedSlot != null)
                    {
                        preDeleteSlot = currentDraggedSlot;
                        deletePropButtonTransform.GetComponent<Image>().color = Color.red;
                        if (startDragSlot)
                        { startDragSlot.Despawn(); }
                        SwitchSlotTo(null);
                    }
                    closestSlot = null;
                    shouldBeDeleted = true;
                }
                else
                {
                    if (preDeleteSlot != null)
                    {
                        closestSlot = preDeleteSlot;
                        preDeleteSlot = null;
                    }
                    deletePropButtonTransform.GetComponent<Image>().color = new Color(188 / 255f, 81f / 255f, 177 / 255f);
                }

                // Place the object there (change slot)
                if (closestSlot != currentDraggedSlot && closestSlot != null)
                {
                    SwitchSlotTo(closestSlot);
                }

                // Update highlights
                foreach (var slot in allShopDecorationSlots.Where(x => x.IsAssignableTo(currentDraggedDecoration)))
                {
                    if (slot.HasCurrentlyAssigned(currentDraggedDecoration))
                    {
                        slot.Highlight(true, ShopDecorationSlot.SlotHighlight.Correct);
                    }
                    else if (slot.Assigned)
                    {
                        slot.Highlight(true, ShopDecorationSlot.SlotHighlight.Wrong);
                    }
                    else
                    {
                        slot.Highlight(true, ShopDecorationSlot.SlotHighlight.Idle);
                    }
                }

                yield return null;

                // Check if we are stopping the dragging
                if (Input.GetMouseButtonUp(0))
                {
                    ReleaseDragPlacement(shouldBeDeleted);
                }
            }
        }

        private void SwitchSlotTo(ShopDecorationSlot newSlot)
        {
            if (currentDraggedSlot != null)
            {
                currentDraggedSlot.Free();
            }

            if (startDragSlot == null)
            {
                startDragSlot = newSlot;
                //Debug.LogWarning("SET START: " + startDragSlot);
            }

            //Debug.Log("Switching to " + newSlot);
            //Debug.Log("Deco is " + currentDraggedDecoration);

            currentDraggedSlot = newSlot;
            if (currentDraggedSlot == null)
            {
                currentDraggedDecoration.transform.position = Vector3.right * 10000;
            }
            else
            {
                currentDraggedSlot.Assign(currentDraggedDecoration);
            }

        }

        private void ReleaseDragPlacement(bool shouldBeDeleted)
        {
            StopDragPlacement();

            // If not dragged on anything
            if (!shouldBeDeleted && currentDraggedSlot == null)
            {
                if (shopContext == ShopContext.NewPlacement)
                {
                    CancelPurchase();
                }
                else
                {
                    CancelMovement();
                }
            }

            if (shouldBeDeleted)
            {
                if (shopContext == ShopContext.NewPlacement)
                {
                    // Cancel the purchase
                    CancelPurchase();
                }
                else if (shopContext == ShopContext.MovingPlacement)
                {
                    // Ask for confirmation
                    AskDeleteConfirmation();
                }
            }
            else
            {
                if (shopContext == ShopContext.NewPlacement)
                {
                    // Ask for confirmation
                    AskPurchaseConfirmation();
                }
                else if (shopContext == ShopContext.MovingPlacement)
                {
                    // Move it right away
                    ConfirmMovement();
                }
            }
        }

        private void AskDeleteConfirmation()
        {
            if (OnDeleteConfirmationRequested != null)
                OnDeleteConfirmationRequested();
        }

        private void AskPurchaseConfirmation()
        {
            if (OnPurchaseConfirmationRequested != null)
                OnPurchaseConfirmationRequested();
        }

        public void ResetSlotHighlights()
        {
            foreach (var shopDecorationSlot in allShopDecorationSlots)
            {
                shopDecorationSlot.Highlight(false);
            }
        }

        public void ResetObjectHighlights()
        {
            foreach (var shopDecorationSlot in allShopDecorationSlots)
            {
                if (shopDecorationSlot.Assigned)
                {
                    shopDecorationSlot.AssignedDecorationObject.FocusHighlight(false);
                }
            }
        }

        void SaveState()
        {
            shopState.occupiedSlots = new ShopSlotState[allShopDecorationSlots.Count];
            for (var index = 0; index < allShopDecorationSlots.Count; index++)
            {
                var slot = allShopDecorationSlots[index];
                //Debug.Log("Check slot: " + slot);
                shopState.occupiedSlots[index] = new ShopSlotState
                {
                    slotType = slot.slotType,
                    slotIndex = slot.slotIndex,
                    decorationID = slot.Assigned ? slot.AssignedDecorationObject.id : ""
                };
                //Debug.Log("NEW SLOT STATE " + shopState.occupiedSlots[index].ToString());
            }

            //Debug.Log(shopState);
            //Debug.Log(shopState.ToJson());
            AppManager.I.Player.Save();
        }

        #endregion

        public void ConfirmPurchase()
        {
            currentDraggedSlot.Spawn();
            if (OnPurchaseComplete != null)
            { OnPurchaseComplete(); }
            SaveState();
            SetContextPurchase();
        }

        public void CancelPurchase()
        {
            if (currentDraggedSlot)
            { currentDraggedSlot.Despawn(); }
            DeleteDecoration(currentDraggedDecoration);
            if (OnPurchaseCancelled != null)
            { OnPurchaseCancelled(); }
            SetContextPurchase();
        }

        public void ConfirmDeletion()
        {
            if (AnturaSpaceScene.I.TutorialMode)
                return;
            DeleteDecoration(currentDraggedDecoration);
            SaveState();
            SetContextPurchase();
        }

        private void CancelMovement()
        {
            SetContextPurchase();
        }

        public void CancelDeletion()
        {
            SwitchSlotTo(startDragSlot);
            SetContextPurchase();
        }

        public void ConfirmMovement()
        {
            currentDraggedSlot.Spawn();
            SaveState();
            SetContextPurchase();
        }
    }
}
