using Common.Services.Net;
using Common.systems.ScriptDirectorSystem.ActorsCasts;

namespace Common.systems.ScriptDirectorSystem.Scripts
{
    public class ConnectionToServerScript : ScriptBase
    {
        public override void Actors(ref ActorCast cast)
        {
            //cast.DefineActor("Net")
            //    .DefineLikeBackEndActor(cast)
            //    .FindExistOrCreate()
            //    .defineRole<INetService>();
        }

        public override void Actions()
        {
            //Actor<INetService>("Net").debug();
        }


    }
}
