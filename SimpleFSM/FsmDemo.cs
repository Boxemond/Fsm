using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class FsmDemo : MonoBehaviour,IFsmAnyState<FsmDemo>
{
    Fsm<FsmDemo> mFsm;
    public string state = "";
    [Button(Name ="Work")]
    void Work()
    {
        Debug.Log("Start Work");
        mFsm.FireEvent(this, Event.work);
    }
    [Button(Name = "Rest")]
    void Rest()
    {
        Debug.Log("Start Rest");
        mFsm.FireEvent(this, Event.rest);
    }
    [Button(Name = "Die")]
    void Die()
    {
        Debug.Log("Start Die");
        mFsm.FireEvent(this, Event.die);
    }

    private void Awake()
    {
        mFsm = Fsm<FsmDemo>.Create("FsmTest", this, this,new NormalState(),new TiredState(),new DeadState());
        mFsm.Start<NormalState>();
    }

    void Update()
    {
        mFsm.Update(Time.deltaTime, Time.unscaledDeltaTime);
    }

    public void OnEvent(IFsm<FsmDemo> fsm, object sender, int eventId, object userData)
    {
        if (eventId == Event.die)
        {
            fsm.ChangeState<DeadState>();
        }
        else
        {
            Debug.Log("No Response!");
        }
    }
    

    class Event
    {
        public const int work = 1;
        public const int rest = 2;
        public const int die = 3;
    }
    class NormalState:FsmState<FsmDemo>
    {
        protected internal override void OnInit(IFsm<FsmDemo> fsm)
        {
            base.OnInit(fsm);
            SubscribeEvent(Event.work, OnWork);
        }
        protected internal override void OnEnter(IFsm<FsmDemo> fsm)
        {
            Debug.Log("NormalState Enter");
            fsm.Owner.state = "现在状态正常";
        }

        protected internal override void OnLeave(IFsm<FsmDemo> fsm, bool isShutdown)
        {
            Debug.Log("NormalState Leave");
        }
        

        bool OnWork(IFsm<FsmDemo> fsm, object sender, object userData)
        {
            fsm.ChangeState<TiredState>();
            return true;
        }
    }

    class TiredState : FsmState<FsmDemo>
    {
        protected internal override void OnInit(IFsm<FsmDemo> fsm)
        {
            base.OnInit(fsm);
            SubscribeEvent(Event.rest, OnRest);
        }
        protected internal override void OnEnter(IFsm<FsmDemo> fsm)
        {
            Debug.Log("TiredState Enter");
            fsm.Owner.state = "现在状态疲惫";
        }

        protected internal override void OnLeave(IFsm<FsmDemo> fsm, bool isShutdown)
        {
            Debug.Log("TiredState Leave");
        }

        bool OnRest(IFsm<FsmDemo> fsm, object sender, object userData)
        {
            fsm.ChangeState<NormalState>();
            return true;
        }
    }
    class DeadState : FsmState<FsmDemo>
    {
        protected internal override void OnInit(IFsm<FsmDemo> fsm)
        {
            base.OnInit(fsm);
            SubscribeEvent(Event.die, OnDead);
        }
        protected internal override void OnEnter(IFsm<FsmDemo> fsm)
        {
            Debug.Log("DeadState Enter");
            fsm.Owner.state = "现在状态死亡";
        }

        protected internal override void OnLeave(IFsm<FsmDemo> fsm, bool isShutdown)
        {
            Debug.Log("DeadState Leave");
        }


        bool OnDead(IFsm<FsmDemo> fsm, object sender, object userData)
        {
            Debug.Log("in DeadState!!!!!");
            return true;
        }

    }

}
