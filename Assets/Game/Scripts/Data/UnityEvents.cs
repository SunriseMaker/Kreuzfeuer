using UnityEngine.Events;

namespace Game.Data
{
	public sealed class UnityEventActor : UnityEvent<Actor>{}

	public sealed class UnityEventFloat : UnityEvent<float>{}

	public sealed class UnityEventInt : UnityEvent<int>{}
}
