using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Protocol;

namespace BattleDrakeStudios.ModularCharacters {
    public class ModularCharacterEditor : EditorWindow {
        
        private ArmorType _ArmorType;
        private string ArmorName;

        private ModularCharacterManager characterManager;
        private Dictionary<ModularBodyPart, GameObject[]> characterBody;
        private Material characterMaterial;

        private Color primaryColor;
        private Color secondaryColor;
        private Color leatherPrimaryColor;
        private Color leatherSecondaryColor;
        private Color metalPrimaryColor;
        private Color metalSecondaryColor;
        private Color metalDarkColor;
        private Color hairColor;
        private Color skinColor;
        private Color stubbleColor;
        private Color scarColor;
        private Color bodyArtColor;
        private Color eyeColor;

        private string primaryProperty = "_Color_Primary";
        private string secondaryProperty = "_Color_Secondary";
        private string leatherPrimaryProperty = "_Color_Leather_Primary";
        private string leatherSecondaryProperty = "_Color_Leather_Secondary";
        private string metalPrimaryProperty = "_Color_Metal_Primary";
        private string metalSecondaryProperty = "_Color_Metal_Secondary";
        private string metalDarkProperty = "_Color_Metal_Dark";
        private string hairProperty = "_Color_Hair";
        private string skinProperty = "_Color_Skin";
        private string stubbleProperty = "_Color_Stubble";
        private string scarProperty = "_Color_Scar";
        private string bodyArtProperty = "_Color_BodyArt";
        private string eyesProperty = "_Color_Eyes";

        private int helmetCurrentIndex;
        private int helmetMaxIndex;

        private int headAttachmentCurrentIndex;
        private int headAttachmentMaxIndex;

        private int headCurrentIndex;
        private int headMaxIndex;

        private int hatCurrentIndex;
        private int hatMaxIndex;

        private int maskCurrentIndex;
        private int maskMaxIndex;

        private int headCoveringCurrentIndex;
        private int headCoveringMaxIndex;

        private int hairCurrentIndex;
        private int hairMaxIndex;

        private int eyebrowCurrentIndex;
        private int eyebrowMaxIndex;

        private int earCurrentIndex;
        private int earMaxIndex;

        private int facialHairCurrentIndex;
        private int facialHairMaxIndex;

        private int backAttachmentCurrentIndex;
        private int backAttachmentMaxIndex;

        private int torsoCurrentIndex;
        private int torsoMaxIndex;

        private int shoulderAttachmentRightCurrentIndex;
        private int shoulderAttachmentRightMaxIndex;

        private int shoulderAttachmentLeftCurrentIndex;
        private int shoulderAttachmentLeftMaxIndex;

        private int armUpperRightCurrentIndex;
        private int armUpperRightMaxIndex;

        private int armUpperLeftCurrentIndex;
        private int armUpperLeftMaxIndex;

        private int elbowAttachmentRightCurrentIndex;
        private int elbowAttachmentRightMaxIndex;

        private int elbowAttachmentLeftCurrentIndex;
        private int elbowAttachmentLeftMaxIndex;

        private int armLowerRightCurrentIndex;
        private int armLowerRightMaxIndex;

        private int armLowerLeftCurrentIndex;
        private int armLowerLeftMaxIndex;

        private int handRightCurrentIndex;
        private int handRightMaxIndex;

        private int handLeftCurrentIndex;
        private int handLeftMaxIndex;

        private int hipsAttachmentCurrentIndex;
        private int hipsAttachmentMaxIndex;

        private int hipsCurrentIndex;
        private int hipsMaxIndex;

        private int kneeAttachmentRightCurrentIndex;
        private int kneeAttachmentRightMaxIndex;

        private int kneeAttachmentLeftCurrentIndex;
        private int kneeAttachmentLeftMaxIndex;

        private int legRightCurrentIndex;
        private int legRightMaxIndex;

        private int legLeftCurrentIndex;
        private int legLeftMaxIndex;

        private Vector2 scrollPos;
        private bool selectionInitialized;

        [MenuItem("BattleDrakeStudios/ModularCharacter/CharacterEditor")]
        public static void ShowWindow() {
            ModularCharacterEditor editorWindow = EditorWindow.GetWindow<ModularCharacterEditor>();
            editorWindow.titleContent = new GUIContent("Modular Character Editor");
            editorWindow.Show();
        }

        private void OnEnable() {
            Undo.undoRedoPerformed += UndoPerformed;
        }
        private void OnDisable() {
            Undo.undoRedoPerformed -= UndoPerformed;
        }

        private void UndoPerformed() {
            if (Selection.activeGameObject) {
                characterManager = Selection.activeGameObject.GetComponent<ModularCharacterManager>();
                if (characterManager != null) {
                    if (characterManager.IsInitialized) {
                        InitializeColors();
                    }
                }
            }
        }

        private void OnSelectionChange() {
            selectionInitialized = false;

            if (Selection.activeGameObject != null) {
                characterManager = Selection.activeGameObject.GetComponent<ModularCharacterManager>();
                if (characterManager != null) {
                    if (characterManager.IsInitialized) {
                        InitializeBodyParts();
                        InitializeColors();
                    }

                }
            }
            Repaint();
        }

        private void OnInspectorUpdate() {
            if (Selection.activeGameObject != null) {
                if (characterManager == null) {
                    characterManager = Selection.activeGameObject.GetComponent<ModularCharacterManager>();

                }
                if (characterManager != null)
                    if (characterManager.gameObject == Selection.activeGameObject) {
                        if (characterManager.IsInitialized) {
                            if (!selectionInitialized) {
                                InitializeBodyParts();
                                InitializeColors();
                            }
                        }
                    } else {
                        selectionInitialized = false;
                        characterManager = Selection.activeGameObject.GetComponent<ModularCharacterManager>();
                    }

                Repaint();
            }
        }

        private void InitializeBodyParts() {
            characterBody = characterManager.GetCharacterBody();

            SetupBodyPart(ModularBodyPart.头盔, ref helmetCurrentIndex, ref helmetMaxIndex);
            SetupBodyPart(ModularBodyPart.头部附件, ref headAttachmentCurrentIndex, ref headAttachmentMaxIndex);
            SetupBodyPart(ModularBodyPart.头部, ref headCurrentIndex, ref headMaxIndex);
            SetupBodyPart(ModularBodyPart.帽子, ref hatCurrentIndex, ref hatMaxIndex);
            SetupBodyPart(ModularBodyPart.面具, ref maskCurrentIndex, ref maskMaxIndex);
            SetupBodyPart(ModularBodyPart.头套, ref headCoveringCurrentIndex, ref headCoveringMaxIndex);
            SetupBodyPart(ModularBodyPart.头发, ref hairCurrentIndex, ref hairMaxIndex);
            SetupBodyPart(ModularBodyPart.眉, ref eyebrowCurrentIndex, ref eyebrowMaxIndex);
            SetupBodyPart(ModularBodyPart.耳朵, ref earCurrentIndex, ref earMaxIndex);
            SetupBodyPart(ModularBodyPart.面部毛发, ref facialHairCurrentIndex, ref facialHairMaxIndex);
            SetupBodyPart(ModularBodyPart.背部附件, ref backAttachmentCurrentIndex, ref backAttachmentMaxIndex);
            SetupBodyPart(ModularBodyPart.躯干, ref torsoCurrentIndex, ref torsoMaxIndex);
            SetupBodyPart(ModularBodyPart.右侧肩部附件, ref shoulderAttachmentRightCurrentIndex, ref shoulderAttachmentRightMaxIndex);
            SetupBodyPart(ModularBodyPart.左侧肩部附件, ref shoulderAttachmentLeftCurrentIndex, ref shoulderAttachmentLeftMaxIndex);
            SetupBodyPart(ModularBodyPart.右侧上臂, ref armUpperRightCurrentIndex, ref armUpperRightMaxIndex);
            SetupBodyPart(ModularBodyPart.左侧上臂, ref armUpperLeftCurrentIndex, ref armUpperLeftMaxIndex);
            SetupBodyPart(ModularBodyPart.右侧手臂关节, ref elbowAttachmentRightCurrentIndex, ref elbowAttachmentRightMaxIndex);
            SetupBodyPart(ModularBodyPart.左侧手臂关节, ref elbowAttachmentLeftCurrentIndex, ref elbowAttachmentLeftMaxIndex);
            SetupBodyPart(ModularBodyPart.右侧下臂, ref armLowerRightCurrentIndex, ref armLowerRightMaxIndex);
            SetupBodyPart(ModularBodyPart.左侧下臂, ref armLowerLeftCurrentIndex, ref armLowerLeftMaxIndex);
            SetupBodyPart(ModularBodyPart.右手, ref handRightCurrentIndex, ref handRightMaxIndex);
            SetupBodyPart(ModularBodyPart.左手, ref handLeftCurrentIndex, ref handLeftMaxIndex);
            SetupBodyPart(ModularBodyPart.臀部_腰部附件, ref hipsAttachmentCurrentIndex, ref hipsAttachmentMaxIndex);
            SetupBodyPart(ModularBodyPart.臀部_腰部, ref hipsCurrentIndex, ref hipsMaxIndex);
            SetupBodyPart(ModularBodyPart.右侧膝盖, ref kneeAttachmentRightCurrentIndex, ref kneeAttachmentRightMaxIndex);
            SetupBodyPart(ModularBodyPart.左侧膝盖, ref kneeAttachmentLeftCurrentIndex, ref kneeAttachmentLeftMaxIndex);
            SetupBodyPart(ModularBodyPart.右脚, ref legRightCurrentIndex, ref legRightMaxIndex);
            SetupBodyPart(ModularBodyPart.左脚, ref legLeftCurrentIndex, ref legLeftMaxIndex);

            selectionInitialized = true;
        }

        private void InitializeColors() {
            characterMaterial = characterManager.CharacterMaterial;

            primaryColor = characterMaterial.GetColor(primaryProperty);
            secondaryColor = characterMaterial.GetColor(secondaryProperty);
            leatherPrimaryColor = characterMaterial.GetColor(leatherPrimaryProperty);
            leatherSecondaryColor = characterMaterial.GetColor(leatherSecondaryProperty);
            metalPrimaryColor = characterMaterial.GetColor(metalPrimaryProperty);
            metalSecondaryColor = characterMaterial.GetColor(metalSecondaryProperty);
            metalDarkColor = characterMaterial.GetColor(metalDarkProperty);
            hairColor = characterMaterial.GetColor(hairProperty);
            skinColor = characterMaterial.GetColor(skinProperty);
            stubbleColor = characterMaterial.GetColor(stubbleProperty);
            scarColor = characterMaterial.GetColor(scarProperty);
            bodyArtColor = characterMaterial.GetColor(bodyArtProperty);
            eyeColor = characterMaterial.GetColor(eyesProperty);
        }

        private void SetupBodyPart(ModularBodyPart bodyPart, ref int index, ref int maxIndex) {
            if (characterBody.TryGetValue(bodyPart, out GameObject[] partsArray)) {
                maxIndex = partsArray.Length - 1;
                bool hasActivePart = false;
                for (int i = 0; i < partsArray.Length; i++) {
                    if (partsArray[i].activeSelf) {
                        index = i;
                        hasActivePart = true;
                    }
                }
                if (!hasActivePart)
                    index = -1;
            } else {
                index = -1;
            }
        }

        private void OnGUI() {
            if (Selection.activeGameObject != null) {
                if (characterManager != null) {
                    if (!characterManager.IsInitialized) {
                        GUILayout.Label("You must initialize character before using the editor");
                        if (GUILayout.Button("Open Setup Wizard")) {
                            ModularSetupWizard.ShowWizard();
                        }
                        return;
                    }

                    GUILayout.BeginHorizontal();

                    GUILayout.BeginVertical();
                    scrollPos = GUILayout.BeginScrollView(scrollPos);

                    SetupPartSlider(ModularBodyPart.头盔, ref helmetCurrentIndex, helmetMaxIndex);
                    SetupPartSlider(ModularBodyPart.头部附件, ref headAttachmentCurrentIndex, headAttachmentMaxIndex);
                    SetupPartSlider(ModularBodyPart.头部, ref headCurrentIndex, headMaxIndex);
                    SetupPartSlider(ModularBodyPart.帽子, ref hatCurrentIndex, hatMaxIndex);
                    SetupPartSlider(ModularBodyPart.面具, ref maskCurrentIndex, maskMaxIndex);
                    SetupPartSlider(ModularBodyPart.头套, ref headCoveringCurrentIndex, headCoveringMaxIndex);
                    SetupPartSlider(ModularBodyPart.头发, ref hairCurrentIndex, hairMaxIndex);
                    SetupPartSlider(ModularBodyPart.眉, ref eyebrowCurrentIndex, eyebrowMaxIndex);
                    SetupPartSlider(ModularBodyPart.耳朵, ref earCurrentIndex, earMaxIndex);
                    SetupPartSlider(ModularBodyPart.面部毛发, ref facialHairCurrentIndex, facialHairMaxIndex);
                    SetupPartSlider(ModularBodyPart.背部附件, ref backAttachmentCurrentIndex, backAttachmentMaxIndex);
                    SetupPartSlider(ModularBodyPart.躯干, ref torsoCurrentIndex, torsoMaxIndex);
                    SetupPartSlider(ModularBodyPart.右侧肩部附件, ref shoulderAttachmentRightCurrentIndex, shoulderAttachmentRightMaxIndex);
                    SetupPartSlider(ModularBodyPart.左侧肩部附件, ref shoulderAttachmentLeftCurrentIndex, shoulderAttachmentLeftMaxIndex);
                    SetupPartSlider(ModularBodyPart.右侧上臂, ref armUpperRightCurrentIndex, armUpperRightMaxIndex);
                    SetupPartSlider(ModularBodyPart.左侧上臂, ref armUpperLeftCurrentIndex, armUpperLeftMaxIndex);
                    SetupPartSlider(ModularBodyPart.右侧手臂关节, ref elbowAttachmentRightCurrentIndex, elbowAttachmentRightMaxIndex);
                    SetupPartSlider(ModularBodyPart.左侧手臂关节, ref elbowAttachmentLeftCurrentIndex, elbowAttachmentLeftMaxIndex);
                    SetupPartSlider(ModularBodyPart.右侧下臂, ref armLowerRightCurrentIndex, armLowerRightMaxIndex);
                    SetupPartSlider(ModularBodyPart.左侧下臂, ref armLowerLeftCurrentIndex, armLowerLeftMaxIndex);
                    SetupPartSlider(ModularBodyPart.右手, ref handRightCurrentIndex, handRightMaxIndex);
                    SetupPartSlider(ModularBodyPart.左手, ref handLeftCurrentIndex, handLeftMaxIndex);
                    SetupPartSlider(ModularBodyPart.臀部_腰部附件, ref hipsAttachmentCurrentIndex, hipsAttachmentMaxIndex);
                    SetupPartSlider(ModularBodyPart.臀部_腰部, ref hipsCurrentIndex, hipsMaxIndex);
                    SetupPartSlider(ModularBodyPart.右侧膝盖, ref kneeAttachmentRightCurrentIndex, kneeAttachmentRightMaxIndex);
                    SetupPartSlider(ModularBodyPart.左侧膝盖, ref kneeAttachmentLeftCurrentIndex, kneeAttachmentLeftMaxIndex);
                    SetupPartSlider(ModularBodyPart.右脚, ref legRightCurrentIndex, legRightMaxIndex);
                    SetupPartSlider(ModularBodyPart.左脚, ref legLeftCurrentIndex, legLeftMaxIndex);

                    GUILayout.EndScrollView();

                    GUILayout.EndVertical();

                    GUILayout.BeginVertical();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Male")) {
                        characterManager.SwapGender(Gender.Male);
                        InitializeBodyParts();
                    }
                    if (GUILayout.Button("Female")) {
                        characterManager.SwapGender(Gender.Female);
                        InitializeBodyParts();
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginVertical();

                    GUILayout.BeginHorizontal();
                    this._ArmorType = (ArmorType)EditorGUILayout.EnumPopup("ArmorType：", this._ArmorType);
                    this.ArmorName = EditorGUILayout.TextField(this.ArmorName); 
                    if (GUILayout.Button("BuildArmor"))
                    {
                        //Debug.Log("test    !!!");
                        //Debug.Log(this._ArmorType.ToString());
                        this.SavePart(this._ArmorType);
                    }
                    GUILayout.EndHorizontal();  
                    GUILayout.EndVertical();

                    SetupColorFields(ref primaryColor, "Primary Color", primaryProperty);
                    SetupColorFields(ref secondaryColor, "Secondary Color", secondaryProperty);
                    SetupColorFields(ref leatherPrimaryColor, "Leather Primary Color", leatherPrimaryProperty);
                    SetupColorFields(ref leatherSecondaryColor, "Leather Secondary Color", leatherSecondaryProperty);
                    SetupColorFields(ref metalPrimaryColor, "Metal Primary Color", metalPrimaryProperty);
                    SetupColorFields(ref metalSecondaryColor, "Metal Secondary Color", metalSecondaryProperty);
                    SetupColorFields(ref metalDarkColor, "Metal Dark Color", metalDarkProperty);
                    SetupColorFields(ref hairColor, "Hair Color", hairProperty);
                    SetupColorFields(ref skinColor, "Skin Color", skinProperty);
                    SetupColorFields(ref stubbleColor, "Stubble Color", stubbleProperty);
                    SetupColorFields(ref scarColor, "Scar Color", scarProperty);
                    SetupColorFields(ref bodyArtColor, "BodyArt Color", bodyArtProperty);
                    SetupColorFields(ref eyeColor, "Eye Color", eyesProperty);

                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();
                } else {
                    GUILayout.BeginVertical();

                    GUILayout.Label("Target does not have a ModularManager component attached.");
                    if (GUILayout.Button("Open Setup Wizard")) {
                        ModularSetupWizard.ShowWizard();
                    }

                    GUILayout.EndVertical();
                }
            }
        }

        private void SetupColorFields(ref Color partColor, string label, string shaderProperty) {
            EditorGUI.BeginChangeCheck();
            partColor = EditorGUILayout.ColorField(label, partColor);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(characterMaterial, "Undo Color change");
                characterMaterial.SetColor(shaderProperty, partColor);
            }
        }

        private void SetupPartSlider(ModularBodyPart bodyPart, ref int partIndex, int maxIndex) {
            EditorGUI.BeginChangeCheck();
            partIndex = EditorGUILayout.IntSlider(bodyPart.ToString(), partIndex, -1, maxIndex);
            if (EditorGUI.EndChangeCheck()) {
                if (partIndex == -1)
                    characterManager.DeactivatePart(bodyPart);
                else
                    characterManager.ActivatePart(bodyPart, partIndex);
            }

        }
        public void SavePart(ArmorType type)
        {
            int[] partsID = new int[(int)ModularBodyPart.Max];
            for(int i = 0; i< partsID.Length; i++)
            {
                partsID[i] = this.characterManager.GetActivePart((ModularBodyPart)i);
                //Debug.LogFormat("部件[{0}] 激活ID为[{1}]", (ModularBodyPart)i, partsID[i]);
            }
            EquipData equipData = AssetDatabase.LoadAssetAtPath<EquipData>(@"Assets/SODatas/Armor Data/EquipData.asset");
            Equip equip = new Equip();
            equip.itemName = this.ArmorName;
            equip.EquipID = equipData.AllEquips.Count;
            equip.armorType = type;
            equip.armorParts = SetArmorParts(type, partsID).ToArray();                  
            AssetDatabase.CreateAsset(equip, @"Assets/SODatas/Armor Data/EquipDatas/" + equip.itemName + ".asset");
            equipData.AllEquips.Add(equip);
        }


        private List<BodyPartLinker> SetArmorParts(ArmorType armorType, int[] partsID)
        {
            List<BodyPartLinker> armorParts = new List<BodyPartLinker>();
            switch (armorType)
            {
                case ArmorType.Helmet:
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.头盔, partsID[(int)ModularBodyPart.头盔]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.头部附件, partsID[(int)ModularBodyPart.头部附件]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.帽子, partsID[(int)ModularBodyPart.帽子]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.面具, partsID[(int)ModularBodyPart.面具]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.头套, partsID[(int)ModularBodyPart.头套]));
                    break;
                case ArmorType.BodyArmor:
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.背部附件, partsID[(int)ModularBodyPart.背部附件]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.躯干, partsID[(int)ModularBodyPart.躯干]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.右侧肩部附件, partsID[(int)ModularBodyPart.右侧肩部附件]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.左侧肩部附件, partsID[(int)ModularBodyPart.左侧肩部附件]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.右侧上臂, partsID[(int)ModularBodyPart.右侧上臂]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.左侧上臂, partsID[(int)ModularBodyPart.左侧上臂]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.臀部_腰部附件, partsID[(int)ModularBodyPart.臀部_腰部附件]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.臀部_腰部, partsID[(int)ModularBodyPart.臀部_腰部]));
                    break;
                case ArmorType.Glove:
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.右侧手臂关节, partsID[(int)ModularBodyPart.右侧手臂关节]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.左侧手臂关节, partsID[(int)ModularBodyPart.左侧手臂关节]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.右侧下臂, partsID[(int)ModularBodyPart.右侧下臂]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.左侧下臂, partsID[(int)ModularBodyPart.左侧下臂]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.右手, partsID[(int)ModularBodyPart.右手]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.左手, partsID[(int)ModularBodyPart.左手]));
                    break;
                case ArmorType.Legs:
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.右侧膝盖, partsID[(int)ModularBodyPart.右侧膝盖]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.左侧膝盖, partsID[(int)ModularBodyPart.左侧膝盖]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.右脚, partsID[(int)ModularBodyPart.右脚]));
                    armorParts.Add(new BodyPartLinker(ModularBodyPart.左脚, partsID[(int)ModularBodyPart.左脚]));
                    break;
            }
            return armorParts;
        }
    }
}
