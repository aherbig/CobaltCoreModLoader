﻿using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using DemoMod.StoryStuff;
using Microsoft.Extensions.Logging;

namespace DemoMod
{
    internal class DemoStoryManifest : IStoryManifest
    {
        public static ExternalPartType? DemoPartType { get; private set; }
        public IEnumerable<DependencyEntry> Dependencies => new DependencyEntry[0];
        public DirectoryInfo? GameRootFolder { get; set; }
        public ILogger? Logger { get; set; }
        public DirectoryInfo? ModRootFolder { get; set; }
        public string Name => "EWanderer.Demomod.DemoStoryManifest";

        public void LoadManifest(IStoryRegistry storyRegistry)
        {
            // A combat shout
            var exampleShout = new ExternalStory("EWanderer.Demomod.DemoStory.CombatShout",
                new StoryNode() // Native CobaltCore class, containing numerous options regarding the shout's trigger. Listed are only the most common, but feel free to explore
                {
                    type = NodeType.combat, // Mark the story as a combat shout
                    priority = true, // Forces this story to be selected before other valid ones when the database is queried, useful for debugging.

                    once = false,
                    oncePerCombat = false,
                    oncePerRun = false, // Self explanatory

                    lookup = new HashSet<string>() // This is a list of tags that queries look for in various situations, very useful for triggering shouts in specific situations !
                    {
                        "demoCardShout" // We'll feed this string to a CardAction's dialogueSelector field in EWandererDemoCard, so that this shout triggers when we play the upgrade B of the card
                    },
                    
                    allPresent = new HashSet<string>() // this checks for the presence of a list of characters.
                    {
                        "riggs"
                    }
                },
                new List<object>() /* this is the actual dialogue. You can feed this list :
                                    * classes inheriting from Instruction (natively Command, Say, or Sayswitch)
                                    * ExternalStory.ExternalSay, which act as a native Say, but automating the more tedious parts,
                                    * such as localizing and hashing*/
                {
                    new ExternalStory.ExternalSay()
                    {
                        who = "riggs", /* the character that talks. For modded characters, use CharacterDeck.GlobalName
                                        * attempting to make an absent character speak in combat will interrupt the shout !*/
                        what = "Example shout !",
                        loopTag = "squint" // the specific animation that should play during the shout. "neutral" is default
                    },
                    new Say() // same as above, but native
                    {
                        who = "peri",
                        hash = "0" // a string that must be unique to your story, used to fetch localisation 
                    },
                    new SaySwitch() /* this is used to randomly pick a valid options among the listed Says.
                                     * Currently doesn't support ExternalStory.ExternalSay, so you'll have to input localisation and hashing by hand.*/
                    {
                        lines = new List<Say> { 
                            new Say() { 
                                who = "max",
                                hash = "maxSwitch"
                            },
                            new Say() {
                                who = "eunice",
                                hash = "drakeSwitch"
                            },
                            new Say() {
                                who = "books",
                                hash = "booksSwitch"
                            },
                        }
                    }
                }
            );
            exampleShout.AddLocalisation("0", "Example native shout !"); // setting the localisation for peri's shout using the native way
            exampleShout.AddLocalisation("maxSwitch", "Rad");
            exampleShout.AddLocalisation("drakeSwitch", "Im super mean btw");
            exampleShout.AddLocalisation("booksSwitch", "*eats crystal shard*");

            storyRegistry.RegisterStory(exampleShout);

            var exampleEvent = new ExternalStory("EWanderer.Demomod.DemoStory.ChoiceEvent",
                    node: new StoryNode()
                    {
                        type = NodeType.@event,// Mark the story as an event
                        priority = true,
                        canSpawnOnMap = true, // self explanatory, dictate whether the event can be a [?] node on the map

                        zones = new HashSet<string>() // dictate in which zone of the game the event can trigger.
                        {
                            "zone_first"
                            //"zone_lawless"
                            //"zone_three"
                            //"zone_magic"
                            //"zone_finale"
                        },

                        /*lookup = new HashSet<string>() 
                        {
                            Lookup for event have some interesting functionalities. For exemple, the tag before_[EnemyName] or after_[EnemyName] will
                            make it so the event triggers right before or after said enemy combat, as done with the mouse knight guy for example
                        },*/

                        choiceFunc = "demoChoiceFunc", /* This triggers a registered choice function at the end of the dialogue.
                                                        * You can see vanilla examples in the class Events*/
                    },
                    instructions: new List<object>()
                    {
                        new ExternalStory.ExternalSay()
                        {
                            who = "walrus", // characters in event dialogues don't need to be actually present !
                            what = "Example event start !",
                            flipped = true, // if true, the character is on the right side of the dialogue while talking
                        },
                        new Command(){name = "demoDoStuffCommand"}, /* execute a registered method, only works during dialogues.
                                                                     * You can see vanilla examples in the class StoryCommands*/
                        new ExternalStory.ExternalSay()
                        {
                            who = "comp",
                            what = "Ouch !",
                        },
                    }
                );

            storyRegistry.RegisterChoice("demoChoiceFunc", typeof(DemoStoryChoices).GetMethod(nameof(DemoStoryChoices.DemoStoryChoice)));
            storyRegistry.RegisterCommand("demoDoStuffCommand", typeof(DemoStoryCommands).GetMethod(nameof(DemoStoryCommands.DemoStoryCommand)));
            storyRegistry.RegisterStory(exampleEvent);

            var exampleEventOutcome_0 = new ExternalStory("EWanderer.Demomod.DemoStory.ChoiceEvent_Outcome_0",
                    node: new StoryNode()
                    {
                        type = NodeType.@event,
                    },
                    instructions: new List<object>()
                    {
                        new ExternalStory.ExternalSay()
                        {
                            who = "comp",
                            what = "Yay !",
                        },
                    }
                );
            storyRegistry.RegisterStory(exampleEventOutcome_0);

            var exampleEventOutcome_1 = new ExternalStory("EWanderer.Demomod.DemoStory.ChoiceEvent_Outcome_1",
                    node: new StoryNode()
                    {
                        type = NodeType.@event,
                    },
                    instructions: new List<object>()
                    {
                        new ExternalStory.ExternalSay()
                        {
                            who = "comp",
                            what = "That hurts !",
                        },
                    }
                );
            storyRegistry.RegisterStory(exampleEventOutcome_1);

            var exampleEventOutcome_2 = new ExternalStory("EWanderer.Demomod.DemoStory.ChoiceEvent_Outcome_2",
                    node: new StoryNode()
                    {
                        type = NodeType.@event
                    },
                    instructions: new List<object>()
                    {
                        new ExternalStory.ExternalSay()
                        {
                            who = "comp",
                            what = "Let's scoot !",
                        },
                    }
                );
            storyRegistry.RegisterStory(exampleEventOutcome_2);
        }
    }
}
