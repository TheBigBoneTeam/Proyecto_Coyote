using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;
using BehaviourAPI.BehaviourTrees;

public class TEstBehaviour : BehaviourRunner
{
	[SerializeField] private testColorChange m_testColorChange;
	
	protected override void Init()
	{
		m_testColorChange = GetComponent<testColorChange>();
		
		base.Init();
	}
	
	protected override BehaviourGraph CreateGraph()
	{
		FSM newbehaviourgraph = new FSM();
		BehaviourTree BehaviourTreeNivel1 = new BehaviourTree();
		FSM subFSMCOlor = new FSM();
		
		SubsystemAction Main_action = new SubsystemAction(BehaviourTreeNivel1);
		State Main = newbehaviourgraph.CreateState(Main_action);
		
		TalkAction Secondary_action = new TalkAction();
		State Secondary = newbehaviourgraph.CreateState(Secondary_action);
		
		UnityTimePerception aa_perception = new UnityTimePerception();
		aa_perception.TotalTime = 5f;
		StateTransition aa = newbehaviourgraph.CreateTransition(Main, Secondary, aa_perception);
		
		TalkAction unnamed_action = new TalkAction();
		State unnamed = newbehaviourgraph.CreateState(unnamed_action);
		
		UnityTimePerception unnamed_1_perception = new UnityTimePerception();
		unnamed_1_perception.TotalTime = 2f;
		StateTransition unnamed_1 = newbehaviourgraph.CreateTransition(Secondary, unnamed, unnamed_1_perception);
		
		UnityTimePerception unnamed_2_perception = new UnityTimePerception();
		unnamed_2_perception.TotalTime = 2f;
		StateTransition unnamed_2 = newbehaviourgraph.CreateTransition(unnamed, Main, unnamed_2_perception);
		
		FunctionalAction Blue_action = new FunctionalAction();
		Blue_action.onUpdated = m_testColorChange.Blue;
		LeafNode Blue = BehaviourTreeNivel1.CreateLeafNode(Blue_action);
		
		DelayAction unnamed_5_action = new DelayAction();
		unnamed_5_action.delayTime = 0.2f;
		LeafNode unnamed_5 = BehaviourTreeNivel1.CreateLeafNode(unnamed_5_action);
		
		FunctionalAction Red_action = new FunctionalAction();
		Red_action.onUpdated = m_testColorChange.Red;
		LeafNode Red = BehaviourTreeNivel1.CreateLeafNode(Red_action);
		
		DelayAction unnamed_6_action = new DelayAction();
		unnamed_6_action.delayTime = 0.2f;
		LeafNode unnamed_6 = BehaviourTreeNivel1.CreateLeafNode(unnamed_6_action);
		
		FunctionalAction unnamed_7_action = new FunctionalAction();
		unnamed_7_action.onUpdated = m_testColorChange.Shit;
		LeafNode unnamed_7 = BehaviourTreeNivel1.CreateLeafNode(unnamed_7_action);
		
		SequencerNode unnamed_4 = BehaviourTreeNivel1.CreateComposite<SequencerNode>(false, Blue, unnamed_5, Red, unnamed_6, unnamed_7);
		unnamed_4.IsRandomized = false;
		
		LoopNode unnamed_3 = BehaviourTreeNivel1.CreateDecorator<LoopNode>(unnamed_4);
		unnamed_3.Iterations = -1;
		
		return newbehaviourgraph;
	}
}
