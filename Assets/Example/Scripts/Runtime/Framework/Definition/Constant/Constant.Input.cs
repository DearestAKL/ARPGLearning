namespace GameMain.Runtime
{
    public static partial class Constant
    {
        public static class InputDef
        {
            //TODO:研究一下手柄适配，以及命名
            public const string Look = "Look";
            public const string Move = "Move";
            public const string Scroll = "Scroll";
            
            //Common 通用
            public const string Interaction = "Interaction";//交互 手柄A 键盘F
            
            //Battle 战斗
            public const string BasicAttack = "BasicAttack";//普通攻击 鼠标左键 手柄X ====Tap=>Hold====
            public const string Dash = "Dash";//冲刺，点按冲刺/长按奔跑 鼠标右键 手柄A ====Tap=>Hold====
            //public const string Dodge = "Dodge";//闪避，点按闪避/长按奔跑 鼠标右键 手柄A ====Tap=>Hold====
            public const string SpecialAttack = "SpecialAttack";//特殊攻击 键盘E 手柄Y ====Tap====
            public const string Ultimate = "Ultimate";//终结技 键盘Q 手柄RT ====Tap====
            
            //UI
            //手柄需要使用组合按键来适配不同界面
            public const string Return = "Return";//返回 键盘Esc 手柄B
            public const string Menu = "Menu";//菜单 键盘Esc 手柄菜单键（三个横杆
            public const string Backpack = "Backpack";//键盘B
            
            public const string Test = "Test";//测试按键 键盘Shift+K
            public const string Fishing = "Fishing";//钓鱼 鼠标左键
        }
    }
}