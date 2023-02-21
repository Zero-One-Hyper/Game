
namespace BattleDrakeStudios.ModularCharacters {
    public static class ModularExtensionMethods {
        public static bool IsHead(this ModularBodyPart part) {
            if (part == ModularBodyPart.头部) {
                return true;
            }
            return false;
        }

        public static bool IsHeadPart(this ModularBodyPart part) {
            if (part == ModularBodyPart.头发 || part == ModularBodyPart.眉 || part == ModularBodyPart.耳朵 || part == ModularBodyPart.面部毛发) {
                return true;
            }
            return false;
        }

        public static bool IsBaseBodyPart(this ModularBodyPart part) {
            if (part == ModularBodyPart.头部 || part == ModularBodyPart.躯干 || part == ModularBodyPart.右侧上臂 || part == ModularBodyPart.左侧上臂 ||
                    part == ModularBodyPart.右侧下臂 || part == ModularBodyPart.左侧下臂 || part == ModularBodyPart.右手 || part == ModularBodyPart.左手 ||
                    part == ModularBodyPart.臀部_腰部 || part == ModularBodyPart.右脚 || part == ModularBodyPart.左脚) {
                return true;
            }
            return false;
        }
    }
}
