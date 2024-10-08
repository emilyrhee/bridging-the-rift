using Godot;
using System;
using System.Threading.Tasks;

public partial class CutsceneArea : Area2D
{
    private RichTextLabel dialog;
    private bool playerIsNearby = false;
    [Export] Font font;
    private int dialogState = 0;
    
    public override void _Ready()
    {
        dialog = GetNode<RichTextLabel>("Dialog");
        dialog.PushFont(font);
        dialog.PushFontSize(9);
    }
    private void ResetTextTo(string text)
    {
        dialog.Clear();
        dialog.PushFont(font);
        dialog.PushFontSize(8);
        dialog.AddText(text);
    }
    public void _on_body_entered(Node2D body)
    {
        if (body is Player) // argument MUST be a Node2D, hence the node type MUST be checked
        {
            dialog.AddText("Hello! How are you?");

            Player.speed = 0f;

            playerIsNearby = true;
        }
    }
    private async Task Sleep(int milliseconds)
    {
        await Task.Delay(milliseconds);
    }
    private float smolJump = -150.0f;
    public override async void _Input(InputEvent @event)
    {
        if (playerIsNearby && @event is InputEventKey eventKey && eventKey.Pressed && Input.IsActionJustPressed("ui_accept"))
        {
            Player player = GetNode<Player>("../../Player");
            switch (dialogState)
            {
                case 0:
                    player.JumpWithVelocity(smolJump);
                    await Sleep(1000);
                    ResetTextTo("Me too. That meteor storm sure did a number on us.");
                    dialogState++;
                    break;
                case 1:
                    await Sleep(500);
                    ResetTextTo("It's been hard gathering resources for my crew to build bridges.");
                    dialogState++;
                    break;
                case 2:
                    player.JumpWithVelocity(smolJump);
                    await Sleep(1000);
                    ResetTextTo("You've been collecting wood?! Would you have any to spare?");
                    dialogState++;
                    break;
                default:
                    Player.isNodding = true;
                    await Sleep(1000);
                    Player.isNodding = false;
                    ResetTextTo("Thanks a lot! My crew will get to building immediately!");
                    break;
            }
        }
    }
}