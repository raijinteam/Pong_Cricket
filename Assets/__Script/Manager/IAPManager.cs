using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour,IStoreListener {

    public static IAPManager Instance;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    public static string[] Products = {  "com.DefaultCompany.PongCricket.GamePack0",
                                        "com.DefaultCompany.PongCricket.GamePack1",
                                        "com.DefaultCompany.PongCricket.GamePack2",
                                        "com.DefaultCompany.PongCricket.GamePack3",
                                        "com.DefaultCompany.PongCricket.GamePack4",
                                         "com.DefaultCompany.PongCricket.GamePack5",
                                        "com.DefaultCompany.PongCricket.GamePack6",
                                        "com.DefaultCompany.PongCricket.GamePack7",
                                       };

    private void Awake() {


        Instance = this;
    }

    void Start() {
        if (m_StoreController == null) {
            InitializePurchasing();
        }
    }

    private void Update() {
       
        if (Input.GetKeyDown(KeyCode.Space)) {

            Debug.Log("Hii");
            foreach (var product in m_StoreController.products.all) {
                Debug.Log(string.Format("string: {0}", product.metadata.localizedPriceString));

                Debug.Log(string.Format("decimal: {0}", product.metadata.localizedPrice.ToString()));
            }
        }
    }

    public string getLocalPriceData(int index) {


        int i = 0;
        string str = null;
        foreach (var product in m_StoreController.products.all) {
            i++;
            if (index ==  i) {
              
                decimal price = product.metadata.localizedPrice;
                string code = product.metadata.isoCurrencyCode;
                str =  price + " __" + code;
                break;
            }
        }

        Debug.Log(str + "... ProducetData");
        return str;
    }


    public void InitializePurchasing() {
        if (IsInitialized()) {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        for (int i = 0; i < Products.Length; i++) {
            builder.AddProduct(Products[i], ProductType.Consumable);
        }
        UnityPurchasing.Initialize(this, builder);

       
    }


    private bool IsInitialized() {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    public void BuyConsumable(int index) {

        Debug.Log("IAP" + index);
        BuyProductID(Products[index]);
    }



    void BuyProductID(string productId) {
        if (IsInitialized()) {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase) {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }



    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
        Debug.Log("OnInitialized: PASS");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error) {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
        if (String.Equals(args.purchasedProduct.definition.id, Products[0], StringComparison.Ordinal)) {

            UIManager.Instance.ui_ShopScreen.GetReward(0);
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Products[1], StringComparison.Ordinal)) {
            UIManager.Instance.ui_ShopScreen.GetReward(1);
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Products[2], StringComparison.Ordinal)) {

            UIManager.Instance.ui_ShopScreen.GetReward(2);
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Products[3], StringComparison.Ordinal)) {
            UIManager.Instance.ui_ShopScreen.GetReward(3);
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Products[4], StringComparison.Ordinal)) {

            UIManager.Instance.ui_ShopScreen.GetReward(4);
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Products[5], StringComparison.Ordinal)) {

            UIManager.Instance.ui_ShopScreen.GetReward(5);
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Products[6], StringComparison.Ordinal)) {

            UIManager.Instance.ui_ShopScreen.GetReward(6);
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Products[7], StringComparison.Ordinal)) {

            UIManager.Instance.ui_ShopScreen.GetReward(7);
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        }

        else {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {

        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message) {


    }
}

