using System;
using System.Threading;

namespace RootNode
{
	class MainClass
	{
		private static readonly Random mRandom = new Random();

		public static void PopulateNode(Node node, int total_levels, int current_level, bool loop = false) {
			//populate RootNode tree
			bool pass = Convert.ToBoolean(mRandom.Next(0, 2));
			if (node == null || (loop && !pass) || current_level > total_levels)
				return;
			int row = mRandom.Next (1, 11);
			node.Children = new Node[row];
			current_level++;
			for (int i = 0; i < row; i++) {
				node.Children [i] = new Node ();
				PopulateNode (node.Children [i], total_levels, current_level, true);
			}
		}

		public static void SetRightProperty(Node root_node, Node previous_node) {
			//Set Right property
			if (root_node == null || root_node.Children == null || root_node.Children.Length < 1)
				return;
			
			Node left = null;
			if (previous_node != null)
				previous_node.Right = root_node.Children [0];
			previous_node = null;

			foreach (Node child in root_node.Children) {
				if (left != null) {
					left.Right = child;
				} 

				SetRightProperty (child, previous_node);
				left = child;

				if (child != null && child.Children != null && child.Children.Length > 0)
					previous_node = child.Children[child.Children.Length-1];
			}
		}

		public static bool TestNode(Node node) {
			//Make sure Right property is correctly set 
			if (node == null || node.Children == null || node.Children.Length < 1)
				return true;
			
			Guid? child_id_right = null;
			Guid? id_right = null;

			foreach (Node child in node.Children) {
				bool child_passed = child != null && child.Children != null && child.Children.Length > 0 && child.Children[child.Children.Length-1].Right != null;
				bool passed = child != null && child.Right != null;

				if (child_id_right.HasValue && child_passed) {
					if (child_id_right.Value == child.Children [0].id)
						Console.WriteLine ("Child test passed: " + child_id_right.Value.ToString ("N") + " == " + child.Children [0].id.ToString ("N"));
					else {
						Console.WriteLine ("Child test not passed: " + child_id_right.Value.ToString ("N") + " != " + child.Children [0].id.ToString ("N"));
						return false;
					}
				}

				if(id_right.HasValue && passed) {
					if (id_right.Value == child.id)
						Console.WriteLine ("Test passed: " + id_right.Value.ToString ("N") + " == " + child.Right.id.ToString ("N"));
					else {
						Console.WriteLine ("Test not passed: " + id_right.Value.ToString ("N") + " != " + child.Right.id.ToString ("N"));
						return false;
					}
				}

				if (child_passed)
					child_id_right = child.Children[child.Children.Length-1].Right.id;
				if(passed)
					id_right = child.Right.id;

				if (!TestNode (child))
					return false;
			}

			return true;
		}

		public static void Main (string[] args)
		{

			for (int i = 1; i <= 10; i++) {
				Console.WriteLine ("Start testing iteration " + i);
				Thread.Sleep (2000);
				int levels = mRandom.Next (1, 11);
				Node root_node = new Node ();
				PopulateNode (root_node, levels, 0);
				SetRightProperty (root_node, null);
				if (TestNode (root_node))
					Console.WriteLine ("Success!");
				else
					Console.WriteLine ("Failed!");
				Console.WriteLine ("Finished testing iteration " + i + "===========================================================================================\n");
				Thread.Sleep (3000);
			}
		}


	}
}
