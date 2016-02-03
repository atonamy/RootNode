using System;

namespace RootNode
{
	public class Node
	{
		public Guid id;
		public Node[] Children;
		public Node Right;

		public Node() {
			id = Guid.NewGuid ();
			Children = null;
			Right = null;
		}
	}

}

