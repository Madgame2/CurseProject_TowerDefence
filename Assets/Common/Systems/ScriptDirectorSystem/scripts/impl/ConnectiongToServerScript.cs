using Common.systems.ScriptDirectorSystem.ActorsCasts;

namespace Common.systems.ScriptDirectorSystem.Scripts
{
    public class ConnectionToServerScript : ScriptBase
    {
        public override void Actors(ref ActorCast cast)
        {
            cast.DefineActor("NetManager")
                .DefineLikeBackEndActor(cast)
                .defineRole<NetTemp>()
                .FindExistOrCreate();

            cast.DefineActor("MonoBeh")
                .DefineLikeActionActor(cast)
                .defineRole<TestMonobeh>()
                .FindExistOrCreate();
        }

        public override void Actions()
        {
            Actor<NetTemp>("NetManager").Connect();
            ActionActor<TestMonobeh>("MonoBeh").TestMono();
        }


    }
}
