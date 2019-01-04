using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;



public  class TestCommand :SimpleCommand
{
    public override void Execute<SendEntity, Param>(INotification<SendEntity, Param> note)
    {
        Debug.Log(note.NotifiId);
    }
}
