namespace Game
{
    public class RelicAddProcessExample: RelicAddProcess
    {
        public RelicAddProcessExample(SORelic so) : base(so)
        {
        }

        public override void Activate(IRelicSystem sys)
        {
            throw new System.NotImplementedException();
        }

        protected override void TakeEffect(object obj)
        {
            throw new System.NotImplementedException();
        }
    }
}