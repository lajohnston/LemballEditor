namespace LemballEditor.View
{
    public partial class LevelBrowser
    {
        public class LevelGroupItem
        {
            /// <summary>
            /// 
            /// </summary>
            public Model.LevelGroupTypes LevelGroupType { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="levelGroupType"></param>
            public LevelGroupItem(Model.LevelGroupTypes levelGroupType)
            {
                LevelGroupType = levelGroupType;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return LevelGroupType.ToString();
            }
        }
    }
}
