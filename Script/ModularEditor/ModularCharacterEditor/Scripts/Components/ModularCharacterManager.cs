using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleDrakeStudios.ModularCharacters {

    [ExecuteInEditMode]
    public class ModularCharacterManager : MonoBehaviour {

        [Header("Male Base")]
        [SerializeField] private List<BodyPartLinker> maleBaseBody = new List<BodyPartLinker>();

        [Header("Female Base")]
        [SerializeField] private List<BodyPartLinker> femaleBaseBody = new List<BodyPartLinker>();

        [Header("Character Material")]
        [SerializeField] private Material characterMaterial;

        [Header("MaleParts Arrays")]
        [SerializeField] private GameObject[] maleHelmetParts;
        [SerializeField] private GameObject[] maleHeadParts;
        [SerializeField] private GameObject[] maleEyebrowParts;
        [SerializeField] private GameObject[] maleFacialHairParts;
        [SerializeField] private GameObject[] maleTorsoParts;
        [SerializeField] private GameObject[] maleArmUpperRightParts;
        [SerializeField] private GameObject[] maleArmUpperLeftParts;
        [SerializeField] private GameObject[] maleArmLowerRightParts;
        [SerializeField] private GameObject[] maleArmLowerLeftParts;
        [SerializeField] private GameObject[] maleHandRightParts;
        [SerializeField] private GameObject[] maleHandLeftParts;
        [SerializeField] private GameObject[] maleHipsParts;
        [SerializeField] private GameObject[] maleLegRightParts;
        [SerializeField] private GameObject[] maleLegLeftParts;

        [Header("FemaleParts Arrays")]
        [SerializeField] private GameObject[] femaleHelmetParts;
        [SerializeField] private GameObject[] femaleHeadParts;
        [SerializeField] private GameObject[] femaleEyebrowParts;
        [SerializeField] private GameObject[] femaleFacialHairParts;
        [SerializeField] private GameObject[] femaleTorsoParts;
        [SerializeField] private GameObject[] femaleArmUpperRightParts;
        [SerializeField] private GameObject[] femaleArmUpperLeftParts;
        [SerializeField] private GameObject[] femaleArmLowerRightParts;
        [SerializeField] private GameObject[] femaleArmLowerLeftParts;
        [SerializeField] private GameObject[] femaleHandRightParts;
        [SerializeField] private GameObject[] femaleHandLeftParts;
        [SerializeField] private GameObject[] femaleHipsParts;
        [SerializeField] private GameObject[] femaleLegRightParts;
        [SerializeField] private GameObject[] femaleLegLeftParts;

        [Header("UniversalParts Arrays")]
        [SerializeField] private GameObject[] hatParts;
        [SerializeField] private GameObject[] maskParts;
        [SerializeField] private GameObject[] headCoveringParts;
        [SerializeField] private GameObject[] hairParts;
        [SerializeField] private GameObject[] earParts;
        [SerializeField] private GameObject[] headAttachmentParts;
        [SerializeField] private GameObject[] backAttachmentParts;
        [SerializeField] private GameObject[] shoulderAttachmentRightParts;
        [SerializeField] private GameObject[] shoulderAttachmentLeftParts;
        [SerializeField] private GameObject[] elbowAttachmentRightParts;
        [SerializeField] private GameObject[] elbowAttachmentLeftParts;
        [SerializeField] private GameObject[] hipsAttachmentParts;
        [SerializeField] private GameObject[] kneeAttachmentRightParts;
        [SerializeField] private GameObject[] kneeAttachmentLeftParts;

        [HideInInspector]
        [SerializeField] private List<GameObject> allParts = new List<GameObject>();

        [HideInInspector]
        [SerializeField] private bool isInitialized;
        [HideInInspector]
        [SerializeField] private Gender characterGender;

        public bool IsInitialized => isInitialized;
        public Gender CharacterGender => characterGender;
        public Material CharacterMaterial => characterMaterial;

        private Dictionary<ModularBodyPart, GameObject[]> characterBody = new Dictionary<ModularBodyPart, GameObject[]>();
        private Dictionary<ModularBodyPart, int> activeParts = new Dictionary<ModularBodyPart, int>();

        private void OnEnable() {
            if (isInitialized) {
                InitializeDictionaries();
            }
        }

        public void ActivatePart(ModularBodyPart bodyPart, int partID) {
            GameObject partToActivate = GetPartFromID(bodyPart, partID);
            if (partToActivate != null) {
                if (activeParts.TryGetValue(bodyPart, out int activePartID)) {
                    GetPartFromID(bodyPart, activePartID).SetActive(false);
                }
                activeParts[bodyPart] = partID;
                partToActivate.SetActive(true);
            }
        }

        public void DeactivatePart(ModularBodyPart bodyPart) {
            if (activeParts.ContainsKey(bodyPart)) {
                GetPartFromID(bodyPart, activeParts[bodyPart]).SetActive(false);
                activeParts.Remove(bodyPart);
            }
        }
        public void ActiveArmor(ArmorType type, BodyPartLinker[] partIDs)
        {
            switch(type)
            {
                case ArmorType.Helmet:
                    this.SwitchArmorModular(ModularBodyPart.头盔, partIDs[0].partID);
                    this.SwitchArmorModular(ModularBodyPart.头部附件, partIDs[1].partID);
                    this.SwitchArmorModular(ModularBodyPart.帽子, partIDs[2].partID);
                    this.SwitchArmorModular(ModularBodyPart.面具, partIDs[3].partID);
                    this.SwitchArmorModular(ModularBodyPart.头套, partIDs[4].partID);
                    break;
                case ArmorType.BodyArmor:
                    this.SwitchArmorModular(ModularBodyPart.背部附件, partIDs[0].partID);
                    this.SwitchArmorModular(ModularBodyPart.躯干, partIDs[1].partID);
                    this.SwitchArmorModular(ModularBodyPart.右侧肩部附件,partIDs[2].partID);
                    this.SwitchArmorModular(ModularBodyPart.左侧肩部附件,partIDs[3].partID);
                    this.SwitchArmorModular(ModularBodyPart.右侧上臂, partIDs[4].partID);
                    this.SwitchArmorModular(ModularBodyPart.左侧上臂, partIDs[5].partID);
                    this.SwitchArmorModular(ModularBodyPart.臀部_腰部附件,partIDs[6].partID);
                    this.SwitchArmorModular(ModularBodyPart.臀部_腰部, partIDs[7].partID);
                    break;
                case ArmorType.Glove:
                    this.SwitchArmorModular(ModularBodyPart.右侧手臂关节,partIDs[0].partID);
                    this.SwitchArmorModular(ModularBodyPart.左侧手臂关节,partIDs[1].partID);
                    this.SwitchArmorModular(ModularBodyPart.右侧下臂, partIDs[2].partID);
                    this.SwitchArmorModular(ModularBodyPart.左侧下臂, partIDs[3].partID);
                    this.SwitchArmorModular(ModularBodyPart.右手, partIDs[4].partID);
                    this.SwitchArmorModular(ModularBodyPart.左手, partIDs[5].partID);
                    break;
                case ArmorType.Legs:
                    this.SwitchArmorModular(ModularBodyPart.右侧膝盖, partIDs[0].partID);
                    this.SwitchArmorModular(ModularBodyPart.左侧膝盖, partIDs[1].partID);
                    this.SwitchArmorModular(ModularBodyPart.右脚, partIDs[2].partID);
                    this.SwitchArmorModular(ModularBodyPart.左脚, partIDs[3].partID);
                    break;
            }
        }
        private void SwitchArmorModular(ModularBodyPart bodyPart, int partID)
        {
            if ((int)bodyPart > (int)ModularBodyPart.左脚) return;
            if (partID == -1)
                DeactivatePart(bodyPart);
            else
                ActivatePart(bodyPart, partID);/*
            this.DeactivatePart(bodyPart);
            this.ActivatePart(bodyPart, partID);*/
        }

        public void SetPartColor(ModularBodyPart bodyPart, int partID, string colorProperty, Color newColor) {
            GameObject part = GetPartFromID(bodyPart, partID);
            if (part != null) {
                Material tempMaterial = new Material(part.GetComponent<SkinnedMeshRenderer>().sharedMaterial);
                tempMaterial.SetColor(colorProperty, newColor);
                part.GetComponent<SkinnedMeshRenderer>().material = tempMaterial;

            }

        }

        public void SetAllPartsMaterial(Material material) {
            foreach (var part in allParts) {
                part.GetComponent<SkinnedMeshRenderer>().material = material;
            }
        }

        public void SwapGender(Gender bodyGender) {
            this.characterGender = bodyGender;
            foreach (var part in activeParts.ToList()) {
                GetPartFromID(part.Key, part.Value).SetActive(false);
                if (part.Key == ModularBodyPart.眉)
                    activeParts.Remove(part.Key);
            }
            SetupCharacterBodyDictionary();
            foreach (var part in activeParts) {
                GetPartFromID(part.Key, part.Value).SetActive(true);
            }
        }

        public void ToggleBaseBodyDisplay(bool isVisible) {
            switch (characterGender) {
                case Gender.Male:
                    foreach (var part in maleBaseBody) {
                        if (activeParts.ContainsKey(part.bodyType))
                            if (activeParts[part.bodyType] == 0)
                                GetPartFromID(part.bodyType, part.partID).SetActive(isVisible);
                    }
                    break;
                case Gender.Female:
                    foreach (var part in femaleBaseBody) {
                        if (activeParts.ContainsKey(part.bodyType))
                            if (activeParts[part.bodyType] == 0)
                                GetPartFromID(part.bodyType, part.partID).SetActive(isVisible);
                    }
                    break;
            }
        }

        public GameObject GetPartFromID(ModularBodyPart partType, int partID) {
            if (characterBody.TryGetValue(partType, out GameObject[] parts)) {
                return parts[partID];
            }
            Debug.LogError(partType.ToString() + " returned null in ModularManager.GetBodyPart()");
            return null;
        }

        public Dictionary<ModularBodyPart, GameObject[]> GetCharacterBody() {
            return characterBody;
        }

        [ContextMenu("ClearAll")]
        private void ClearAll() {
            SetupNewCharacter(characterGender, characterMaterial);
        }

        public void SetupNewCharacter(Gender characterGender, Material characterMaterial) {
            this.characterMaterial = characterMaterial;

            SetupBodyArrays(false);

            this.characterGender = characterGender;

            InitializeDictionaries();

            List<BodyPartLinker> activeBody = this.characterGender == Gender.Male ? maleBaseBody : femaleBaseBody;
            foreach (var part in activeBody) {
                GetPartFromID(part.bodyType, part.partID).SetActive(true);
                activeParts[part.bodyType] = 0;
            }
        }

        public void SetupExistingCharacter(Gender characterGender, Material characterMaterial) {
            this.characterMaterial = characterMaterial;
            this.characterGender = characterGender;
            SetupBodyArrays(true);
            InitializeDictionaries();
        }

        private void InitializeDictionaries() {
            SetupCharacterBodyDictionary();
            SetupActivePartsDictionary();
        }

        #region ArraySetups
        private void SetupBodyArrays(bool isExisting) {
            SetupPartArray(ref maleHelmetParts, ModularCharacterStatics.MaleHelmetPath, isExisting);
            SetupPartArray(ref maleHeadParts, ModularCharacterStatics.MaleHeadPath, isExisting);
            SetupPartArray(ref maleEyebrowParts, ModularCharacterStatics.MaleEyebrowPath, isExisting);
            SetupPartArray(ref maleFacialHairParts, ModularCharacterStatics.MaleFacialHairPath, isExisting);
            SetupPartArray(ref maleTorsoParts, ModularCharacterStatics.MaleTorsoPath, isExisting);
            SetupPartArray(ref maleArmUpperRightParts, ModularCharacterStatics.MaleArmUpperRightPath, isExisting);
            SetupPartArray(ref maleArmUpperLeftParts, ModularCharacterStatics.MaleArmUpperLeftPath, isExisting);
            SetupPartArray(ref maleArmLowerRightParts, ModularCharacterStatics.MaleArmLowerRightPath, isExisting);
            SetupPartArray(ref maleArmLowerLeftParts, ModularCharacterStatics.MaleArmLowerLeftPath, isExisting);
            SetupPartArray(ref maleHandRightParts, ModularCharacterStatics.MaleHandRightPath, isExisting);
            SetupPartArray(ref maleHandLeftParts, ModularCharacterStatics.MaleHandLeftPath, isExisting);
            SetupPartArray(ref maleHipsParts, ModularCharacterStatics.MaleHipsPath, isExisting);
            SetupPartArray(ref maleLegRightParts, ModularCharacterStatics.MaleLegRightPath, isExisting);
            SetupPartArray(ref maleLegLeftParts, ModularCharacterStatics.MaleLegLeftPath, isExisting);

            SetupPartArray(ref femaleHelmetParts, ModularCharacterStatics.FemaleHelmetPath, isExisting);
            SetupPartArray(ref femaleHeadParts, ModularCharacterStatics.FemaleHeadPath, isExisting);
            SetupPartArray(ref femaleEyebrowParts, ModularCharacterStatics.FemaleEyebrowPath, isExisting);
            SetupPartArray(ref femaleFacialHairParts, ModularCharacterStatics.FemaleFacialHairPath, isExisting);
            SetupPartArray(ref femaleTorsoParts, ModularCharacterStatics.FemaleTorsoPath, isExisting);
            SetupPartArray(ref femaleArmUpperRightParts, ModularCharacterStatics.FemaleArmUpperRightPath, isExisting);
            SetupPartArray(ref femaleArmUpperLeftParts, ModularCharacterStatics.FemaleArmUpperLeftPath, isExisting);
            SetupPartArray(ref femaleArmLowerRightParts, ModularCharacterStatics.FemaleArmLowerRightPath, isExisting);
            SetupPartArray(ref femaleArmLowerLeftParts, ModularCharacterStatics.FemaleArmLowerLeftPath, isExisting);
            SetupPartArray(ref femaleHandRightParts, ModularCharacterStatics.FemaleHandRightPath, isExisting);
            SetupPartArray(ref femaleHandLeftParts, ModularCharacterStatics.FemaleHandLeftPath, isExisting);
            SetupPartArray(ref femaleHipsParts, ModularCharacterStatics.FemaleHipsPath, isExisting);
            SetupPartArray(ref femaleLegRightParts, ModularCharacterStatics.FemaleLegRightPath, isExisting);
            SetupPartArray(ref femaleLegLeftParts, ModularCharacterStatics.FemaleLegLeftPath, isExisting);

            SetupPartArray(ref hatParts, ModularCharacterStatics.HatPath, isExisting);
            SetupPartArray(ref maskParts, ModularCharacterStatics.MaskPath, isExisting);
            SetupPartArray(ref headCoveringParts, ModularCharacterStatics.HeadCoveringPath, isExisting);
            SetupPartArray(ref hairParts, ModularCharacterStatics.HairPath, isExisting);
            SetupPartArray(ref earParts, ModularCharacterStatics.EarPath, isExisting);
            SetupPartArray(ref headAttachmentParts, ModularCharacterStatics.HeadAttachmentPath, isExisting);
            SetupPartArray(ref backAttachmentParts, ModularCharacterStatics.BackAttachmentPath, isExisting);
            SetupPartArray(ref shoulderAttachmentRightParts, ModularCharacterStatics.ShoulderAttachmentRightPath, isExisting);
            SetupPartArray(ref shoulderAttachmentLeftParts, ModularCharacterStatics.ShoulderAttachmentLeftPath, isExisting);
            SetupPartArray(ref elbowAttachmentRightParts, ModularCharacterStatics.ElbowAttachmentRightPath, isExisting);
            SetupPartArray(ref elbowAttachmentLeftParts, ModularCharacterStatics.ElbowAttachmentLeftPath, isExisting);
            SetupPartArray(ref hipsAttachmentParts, ModularCharacterStatics.HipsAttachmentPath, isExisting);
            SetupPartArray(ref kneeAttachmentRightParts, ModularCharacterStatics.KneeAttachmentRightPath, isExisting);
            SetupPartArray(ref kneeAttachmentLeftParts, ModularCharacterStatics.KneeAttachmentLeftPath, isExisting);

            isInitialized = true;
        }

        private void SetupPartArray(ref GameObject[] partsArray, string path, bool isExisting) {
            Transform[] gameObjectTransforms = GetComponentsInChildren<Transform>();

            Transform parentRoot = null;

            foreach (Transform transform in gameObjectTransforms) {
                if (transform.gameObject.name.Equals(path)) {
                    parentRoot = transform;
                    break;
                }
            }

            partsArray = new GameObject[parentRoot.childCount];
            for (int i = 0; i < partsArray.Length; i++) {
                partsArray[i] = parentRoot.GetChild(i).gameObject;
                if (characterMaterial != null)
                    partsArray[i].GetComponent<Renderer>().sharedMaterial = characterMaterial;

                if (partsArray[i].activeSelf)
                    partsArray[i].SetActive(isExisting);

                allParts.Add(partsArray[i]);
            }
        }
        #endregion

        #region DictionarySetups
        private void SetupCharacterBodyDictionary() {
            characterBody.Clear();

            characterBody[ModularBodyPart.帽子] = hatParts;
            characterBody[ModularBodyPart.面具] = maskParts;
            characterBody[ModularBodyPart.头套] = headCoveringParts;
            characterBody[ModularBodyPart.头发] = hairParts;
            characterBody[ModularBodyPart.耳朵] = earParts;
            characterBody[ModularBodyPart.头部附件] = headAttachmentParts;
            characterBody[ModularBodyPart.背部附件] = backAttachmentParts;
            characterBody[ModularBodyPart.右侧肩部附件] = shoulderAttachmentRightParts;
            characterBody[ModularBodyPart.左侧肩部附件] = shoulderAttachmentLeftParts;
            characterBody[ModularBodyPart.右侧手臂关节] = elbowAttachmentRightParts;
            characterBody[ModularBodyPart.左侧手臂关节] = elbowAttachmentLeftParts;
            characterBody[ModularBodyPart.臀部_腰部附件] = hipsAttachmentParts;
            characterBody[ModularBodyPart.右侧膝盖] = kneeAttachmentRightParts;
            characterBody[ModularBodyPart.左侧膝盖] = kneeAttachmentLeftParts;

            if (characterGender == Gender.Male) {
                characterBody[ModularBodyPart.头盔] = maleHelmetParts;
                characterBody[ModularBodyPart.头部] = maleHeadParts;
                characterBody[ModularBodyPart.眉] = maleEyebrowParts;
                characterBody[ModularBodyPart.面部毛发] = maleFacialHairParts;
                characterBody[ModularBodyPart.躯干] = maleTorsoParts;
                characterBody[ModularBodyPart.右侧上臂] = maleArmUpperRightParts;
                characterBody[ModularBodyPart.左侧上臂] = maleArmUpperLeftParts;
                characterBody[ModularBodyPart.右侧下臂] = maleArmLowerRightParts;
                characterBody[ModularBodyPart.左侧下臂] = maleArmLowerLeftParts;
                characterBody[ModularBodyPart.左手] = maleHandRightParts;
                characterBody[ModularBodyPart.右手] = maleHandLeftParts;
                characterBody[ModularBodyPart.臀部_腰部] = maleHipsParts;
                characterBody[ModularBodyPart.右脚] = maleLegRightParts;
                characterBody[ModularBodyPart.左脚] = maleLegLeftParts;
            } else if (characterGender == Gender.Female) {
                characterBody[ModularBodyPart.头盔] = femaleHelmetParts;
                characterBody[ModularBodyPart.头部] = femaleHeadParts;
                characterBody[ModularBodyPart.眉] = femaleEyebrowParts;
                characterBody[ModularBodyPart.面部毛发] = femaleFacialHairParts;
                characterBody[ModularBodyPart.躯干] = femaleTorsoParts;
                characterBody[ModularBodyPart.右侧上臂] = femaleArmUpperRightParts;
                characterBody[ModularBodyPart.左侧上臂] = femaleArmUpperLeftParts;
                characterBody[ModularBodyPart.右侧下臂] = femaleArmLowerRightParts;
                characterBody[ModularBodyPart.左侧下臂] = femaleArmLowerLeftParts;
                characterBody[ModularBodyPart.左手] = femaleHandRightParts;
                characterBody[ModularBodyPart.右手] = femaleHandLeftParts;
                characterBody[ModularBodyPart.臀部_腰部] = femaleHipsParts;
                characterBody[ModularBodyPart.右脚] = femaleLegRightParts;
                characterBody[ModularBodyPart.左脚] = femaleLegLeftParts;
            }
        }

        private void SetupActivePartsDictionary() {
            maleBaseBody.Clear();
            femaleBaseBody.Clear();

            foreach (var bodyPart in characterBody) {
                int activeID = 0;
                foreach (var part in bodyPart.Value) {
                    if (part.activeSelf) {
                        activeParts[bodyPart.Key] = Array.IndexOf(bodyPart.Value, part);
                        activeID = Array.IndexOf(bodyPart.Value, part);
                    }
                }
                if (bodyPart.Key.IsBaseBodyPart()) {
                    SetupBodyLists(bodyPart.Key, activeID);
                }
            }
        }
        #endregion

        #region ListSetup
        private void SetupBodyLists(ModularBodyPart bodyPart, int activeID) {
            if (bodyPart.IsHeadPart()) {
                maleBaseBody.Add(new BodyPartLinker(bodyPart, activeID));
                femaleBaseBody.Add(new BodyPartLinker(bodyPart, activeID));
            } else {
                maleBaseBody.Add(new BodyPartLinker(bodyPart, 0));
                femaleBaseBody.Add(new BodyPartLinker(bodyPart, 0));
            }
        }
        #endregion

        public int GetActivePart(ModularBodyPart part)
        {            
            if(this.activeParts.TryGetValue(part, out int activePart))
            {
                return activePart;
            }
            return -1;
        }
    }
}
