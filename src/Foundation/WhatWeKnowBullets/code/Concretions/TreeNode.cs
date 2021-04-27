﻿using LearnEXM.Foundation.WhatWeKnowBullets.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace LearnEXM.Foundation.WhatWeKnowBullets.Concretions
{
  public class TreeNode : ITreeNode
  {
    public TreeNode(string title)
    {
      Title = title;
    }

    public TreeNode(string title, string value)
    {
      Title = title;
      Value = value;
    }

    public bool HasLeaves
    {
      get
      {
        return Leaves != null && Leaves.Any();
      }
    }

    public List<ITreeNode> Leaves { get; set; } = new List<ITreeNode>();
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public bool ValueIsJson { get; set; } = false;

    public void AddLeaf(ITreeNode treeNode)
    {
      if(treeNode != null)
      {
        Leaves.Add(treeNode);
      }
    }
    public string TitleValue()
    {
      var toReturn = Title;
      if (!string.IsNullOrEmpty(Value))
      {
        toReturn += " : " + Value;
      }
      return toReturn;
    }

    public void AddRawLeaf(string serialized)
    {
      var rawTitleLeaf = (new TreeNode("Raw"));
      rawTitleLeaf.Leaves.Add(new TreeNode(serialized) { ValueIsJson = true });
      Leaves.Add(rawTitleLeaf);
    }
  }
}