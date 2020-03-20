
namespace Model
{
    public partial class ModelManager
    {
        static ModelManager _instance = new ModelManager();

        public static ModelManager Instance { get { return _instance; } }

    }
}