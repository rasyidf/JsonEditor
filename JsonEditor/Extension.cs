using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
namespace JsonEditor
{
    public static class ObjectToTreeView
    {
        private sealed class IndexContainer
        {
            private int _n;
            public int Inc() => _n++;
        }

        private static void FillTreeView(TreeViewItem node, JToken tok, Stack<IndexContainer> s)
        {
            TreeViewItem n = node;
            if (tok.Type == JTokenType.Object)
            {
                if (tok.Parent != null)
                {
                    if (tok.Parent.Type == JTokenType.Property)
                    {
                        n = new TreeViewItem() { Header = $"{((JProperty)tok.Parent).Name} <{tok.Type}>" };
                        node.Items.Add(n);
                    }
                    else
                    {
                        n = new TreeViewItem() { Header = $"[{s.Peek().Inc()}] <{tok.Type}>" };
                        node.Items.Add(n);
                    }
                }
                s.Push(new IndexContainer());
                foreach (var p in tok.Children<JProperty>())
                {
                    FillTreeView(n, p.Value, s);
                }
                s.Pop();
            }
            else if (tok.Type == JTokenType.Array)
            { 
                if (tok.Parent != null)
                {
                    if (tok.Parent.Type == JTokenType.Property)
                    { 
                        n = new TreeViewItem() { Header = $"{((JProperty)tok.Parent).Name} <{tok.Type}>" };
                        node.Items.Add(n);
                    }
                    else
                    { 
                        n = new TreeViewItem() { Header = $"[{s.Peek().Inc()}] <{tok.Type}>" };
                        node.Items.Add(n);
                    }
                }
                s.Push(new IndexContainer());
                foreach (var p in tok)
                {
                    FillTreeView(n, p, s);
                }
                s.Pop();
            }
            else
            {
                var value = JsonConvert.SerializeObject(((JValue)tok).Value);

                string name;
                if (tok.Parent.Type == JTokenType.Property)
                {
                    name = $"{((JProperty)tok.Parent).Name} : {value}";
                }
                else
                {
                    name = $"[{s.Peek().Inc()}] : {value}";
                }

                node.Items.Add(new TreeViewItem() { Header = name });
            }
        }

        public static void SetObjectAsJson<T>(this TreeView tv, T obj)
        {
            tv.BeginInit();
            try
            {
                tv.Items.Clear();

                var s = new Stack<IndexContainer>();
                s.Push(new IndexContainer());
                var root = new TreeViewItem() { Header = "ROOT" };
                FillTreeView(root, JsonConvert.DeserializeObject<JToken>(JsonConvert.SerializeObject(obj)), s);
                tv.Items.Add(root);
                s.Pop();
            }
            finally
            {
                tv.EndInit();
            }
        }
    }
}