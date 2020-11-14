using Akka.Actor;

namespace AkkaClusterExample.Shared
{
    public class ActorMetaData
    {
        public ActorMetaData(string name, ActorMetaData parent = null, bool isSystemActor = false)
        {
            Name = name;
            Parent = parent;
            // if no parent, we assume a top-level actor
            var parentPath = parent != null ? parent.Path : (isSystemActor ? "/system" : "/user");
            Path = $"{parentPath}/{Name}";
        }

        public string Name { get; }
        public ActorMetaData Parent { get; }
        public string Path { get; }

        public ActorSelection Selection(IActorRefFactory context)
        {
            return context.ActorSelection(Path);
        }
    }
}
