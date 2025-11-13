using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDetectiveQuestTracker.Services
{
    public static class ConsoleUi
    {
        
        public static void ShowBriefingPaged(string username)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;

            
            var lines = new List<string>
            {

@"
        ┌────────────────────────────────────────────────────────────────────────────────┐
        │                                  London, 1944                                  │
        ├────────────────────────────────────────────────────────────────────────────────┤
        │ My dear old friend,                                                            │
        │                                                                                │
        │  Word has reached me — through certain acquaintances — that you have taken on  │
        │  far fewer cases of late. Age creeps up on us all, and this dreadful war       │
        │  has a peculiar way of wearing a person out long before their time. The        │
        │  endless rain this autumn has hardly helped matters either.                    │
        │                                                                                │
        │  I shall not draw this out, but come directly to the point:                    │
        │  we need your help.                                                            │
        │                                                                                │
        │  Here at Scotland Yard I find myself utterly overwhelmed. One would hope that, │
        │  given the state of the world, murder might be less common — but alas, evil    │
        │  does not vanish simply because bombs fall elsewhere.                          │
        │                                                                                │
        │  I remember well how you once said that when you finally retired to your flat  │
        │  on Kensington Row, you intended to spend your days in your study, warming     │
        │  yourself by the fire, enjoying a fine cigar while leaving everything else to  │
        │  your butler, George. A peaceful arrangement indeed.                           │
        │                                                                                │
        │  And I fully understand if you do not wish to disturb those plans. I, too,     │
        │  am approaching the age where retirement seems tempting. A quiet cottage in    │
        │  the countryside has begun to sound rather appealing.                          │
        │                                                                                │
        │  But before that day comes, I would be profoundly grateful if you might        │
        │  consider assisting us with a few final cases — with complete freedom to       │
        │  choose only those that capture your interest.                                 │
        │                                                                                │
        │  With warm regards,                                                            │
        │  Commissioner Arthur Penwood                                                   │
        └────────────────────────────────────────────────────────────────────────────────┘
"

            };
            

            foreach (var line in lines)
                
            Console.WriteLine(line);


            {
                Console.ResetColor();
                }
            }
        }
    }

