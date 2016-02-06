using System;
using System.Collections.Generic;
using LotusLibrary.Animation;
using LotusLibrary.DataAccess;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LotusLibrary
{
    public enum EventAction
    {
        TeleportCharacter,
        CreateCharacterMovement,
        AddItems,
        ShowDialog,
        ShowOptions,
        StartQuest,
        FinishGame
    }

    public enum TriggerType
    {
        Activation,
        Interaction,
        Touch
    }

    public class Event : GameObject
    {
        public Event()
        {
        }

        public Event(EventAction action)
        {
            Action = action;
        }

        public Event(EventAction action, int mapId, Vector2 destination, bool fadeScreen)
        {
            Action = action;
            DestinationMapId = mapId;
            Destination = destination;
            FadeScreenOnTeleport = fadeScreen;
        }

        public Event(EventAction eventAction, Quest quest)
        {
            Action = eventAction;
            Quest = quest;
        }

        public int Id { get; set; }
        public int EventMapId { get; set; }

        public string Label
        {
            get { return string.Format("{0}. {1}", Id, Enum.GetName(typeof (EventAction), Action)); }
        }

        public Quest Quest { get; set; }
        public EventAction Action { get; set; }
        public TriggerType TriggerType { get; set; }
        public Vector2 Destination { get; set; }
        public int DestinationMapId { get; set; }
        public bool FadeScreenOnTeleport { get; set; }
        public SpriteAnimation Animation { get; set; }

        public static void Save(Event dialog)
        {
            var dao = new EventDAO();
            if (dialog.Id == 0)
                dialog.Id = dao.Insert(dialog);
            else
                dao.Update(dialog);
        }

        public static List<Event> LoadAll()
        {
            var dao = new EventDAO();
            return dao.SelectAll();
        }

        public static Event Load(int p)
        {
            var dao = new EventDAO();
            return dao.Select(p);
        }

        public override void Draw(SpriteBatch batch)
        {
            if (Animation != null)
            {
                Animation.Draw(batch, MapHelper.GetPixelsFromTileCenter(Position), 100);
            }
        }

        public void Update(GameTime gametime)
        {
            if (Animation != null)
            {
                Animation.Update(gametime);
            }
        }

        public Event Clone()
        {
            return (Event) MemberwiseClone();
        }
    }
}