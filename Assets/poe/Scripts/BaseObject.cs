using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour {
    //消息处理机制
    public delegate void eventDelivery(EventDate kv);
    public Dictionary<string, eventDelivery> _eventList = new Dictionary<string, eventDelivery>();
    //注册事件
    public void OnEvent(string eventType, eventDelivery handler)
    {
        if (!_eventList.ContainsKey(eventType))
        {
            _eventList.Add(eventType, null);
        }
        _eventList[eventType] += handler;
    }
    //取消注册
    public void OffEvent(string eventType, eventDelivery handele)
    {
        if (_eventList.ContainsKey(eventType))
        {
            _eventList[eventType] -= handele;
        }
    }
    //取消所有注册事件
    public void ClearALLMsgListener()
    {
        if (_eventList != null)
        {
            _eventList.Clear();
        }
    }

    //派发事件
    public void Emit(string eventType, object trigger = null)
    {
        eventDelivery call = null;
        if (_eventList.TryGetValue(eventType, out call))
        {
            if (call != null)
            {
                if (trigger == null) trigger = this;
                EventDate data = new EventDate(trigger: trigger);
                call(data);
            }
        }
    }
    //派发事件
    public void Emit(string eventType, EventDate data, object trigger = null)
    {
        eventDelivery call = null;
        if (_eventList.TryGetValue(eventType, out call))
        {
            if (call != null)
            {
                if (trigger == null) trigger = this;
                data.Trigger = trigger;
                call(data);
            }
        }
    }
    //派发事件
    public void Emit(string eventType, string msgName, object msgContent, object trigger = null)
    {
        eventDelivery call = null;
        if (_eventList.TryGetValue(eventType, out call))
        {
            if (call != null)
            {
                Dictionary<string, object> detail = new Dictionary<string, object>();
                detail.Add(msgName, msgContent);
                if (trigger == null) trigger = this;
                EventDate data = new EventDate(detail, trigger);
                call(data);
            }
        }
    }
}


public class EventDate
{
    public Dictionary<string, object> _detail;
    public object _trigger = null;

    public object Detail
    {
        get { return _detail; }
    }
    public object Trigger
    {
        set {
            if(_trigger == null && value != null)
            {
                _trigger = value;
            }
        }
        get { return _trigger; }
    }

    public EventDate(Dictionary<string, object> detail = null, object trigger = null)
    {
        _trigger = trigger;
        _detail = detail;
    }
}
