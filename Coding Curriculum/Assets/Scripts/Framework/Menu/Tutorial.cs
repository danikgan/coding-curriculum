using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {

	public Texture background; //general background

	public GUISkin tutorial_skin = null; //set up skin
	public GUIStyle menu_button_style;
	private Vector2 scrollPositionButtons = Vector2.zero; //vector for scroll buttons scroll area
	private Vector2 scrollPositionText = Vector2.zero; //vector for scroll text  scroll area
	public static int tutorial_to_display = 1; //which tutorial will be displayed by default
	private int num_buttons = 11; //number of buttons in scrollable area
	private int tutorial_text_size;

	void OnGUI () {

		GUI.skin = tutorial_skin; //attach skin
		
		GUI.DrawTexture ( new Rect (0, 0, Screen.width, Screen.height), background); //draw background texture

		tutorial_text_size = (int) (Screen.height * 0.03f);
		tutorial_skin.label.fontSize = tutorial_text_size;

		//setups for button scrollview
		float button_scroll_hor_pos = Screen.width * 0.005f;
		float button_scroll_vert_pos = Screen.height * 0.02f;
		float button_scroll_hor_size = Screen.width * 0.4f;
		float button_scroll_vert_size = Screen.height * 0.8f;
		float gap = Screen.width * 0.06f;


		//setups for text scrollview
		float text_scroll_hor_pos = button_scroll_hor_pos + gap + button_scroll_hor_size;
		float text_scroll_vert_pos = button_scroll_vert_pos;
		float text_scroll_hor_size = Screen.width*0.53f;
		float text_scroll_vert_size = Screen.height*0.9f;


		//set up buttons position and size
		float button_width_pos = 0;
		float button_height_pos = 0;
		float button_width = Screen.width * (1f / 3f);
		float button_height = Screen.height * (1f / 5f);
		float button_gap = Screen.height * (1f / 320f);
		float button_text_size = Screen.height * (1f / 19);

		menu_button_style.fontSize = (int) button_text_size; //set up style for main menu button
		tutorial_skin.button.fontSize = (int) button_text_size; 

		Rect buttonContentRect = new Rect(0, 0, button_scroll_hor_size-20, num_buttons*(button_height+button_gap)); //set button scroll rect size



		//start button scroll
		scrollPositionButtons = GUI.BeginScrollView(new Rect(button_scroll_hor_pos, button_scroll_vert_pos, button_scroll_hor_size, button_scroll_vert_size), scrollPositionButtons, buttonContentRect);


		if (GUI.Button (new Rect (button_width_pos, button_height_pos, button_width, button_height), "Tutorial 1")) {
			tutorial_to_display = 1; 
		}
			
		if (GUI.Button (new Rect (button_width_pos, button_height_pos + button_height + button_gap, button_width, button_height), "Tutorial 2")) {
			tutorial_to_display = 2; 
		}

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + (button_height + button_gap)*2, button_width, button_height), "Tutorial 3")) {
			tutorial_to_display = 3; 
		}

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + (button_height + button_gap)*3, button_width, button_height), "Tutorial 4")) {
			tutorial_to_display = 4; 
		}

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + (button_height + button_gap)*4, button_width, button_height), "Tutorial 5")) {
			tutorial_to_display = 5; 
		}

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + (button_height + button_gap)*5, button_width, button_height), "Tutorial 6")) {
			tutorial_to_display = 6; 
		}

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + (button_height + button_gap)*6, button_width, button_height), "Tutorial 7")) {
			tutorial_to_display = 7; 
		}

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + (button_height + button_gap)*7, button_width, button_height), "Tutorial 8")) {
			tutorial_to_display = 8; 
		}

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + (button_height + button_gap)*8, button_width, button_height), "Tutorial 9")) {
			tutorial_to_display = 9; 
		} 

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + (button_height + button_gap)*9, button_width, button_height), "Tutorial 10")) {
			tutorial_to_display = 10; 
		}

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + (button_height + button_gap)*10, button_width, button_height), "Answers")) {
			tutorial_to_display = 11; 
		}

		GUI.EndScrollView();

		float text_scroll_box_height = Screen.width * 0.7f;

		switch (tutorial_to_display) {

		case 1:
			text_scroll_box_height = Screen.width * 1.1f;
			break;
		case 2:
			text_scroll_box_height = Screen.width * 0.5f;
			break;
		case 3:
			text_scroll_box_height = Screen.width * 0.49f;
			break;
		case 4:
			text_scroll_box_height = Screen.width * 0.49f;
			break;
		case 5:
			text_scroll_box_height = Screen.width * 0.49f;
			break;
		case 6:
			text_scroll_box_height = Screen.width * 0.56f;
			break;
		case 7:
			text_scroll_box_height = Screen.width * 0.72f;
			break;
		case 8:
			text_scroll_box_height = Screen.width * 0.45f;
			break;
		case 9:
			text_scroll_box_height = Screen.width * 0.9f;
			break;
		case 10:
			text_scroll_box_height = Screen.width * 0.65f;
			break;
		case 11:
			text_scroll_box_height = Screen.width * 2f;
			break;
		}




		Rect textContentRect = new Rect (0, 0, text_scroll_hor_size - 20, text_scroll_box_height); //set text scroll rect size

		//start text scroll
		scrollPositionText = GUI.BeginScrollView(new Rect(text_scroll_hor_pos, text_scroll_vert_pos, text_scroll_hor_size, text_scroll_vert_size), scrollPositionText, textContentRect);

		if (tutorial_to_display == 1) {

			GUI.Label (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width*1.1f), "The mars explorer is trying to make his way to the spot marked with \"X\" to collect samples of martian soil, but it cannot do that alone, it needs your help! \n\nYour mission is to control that robot by giving it instructions.\nOn this level, the only instruction available to you is “moveForward”, which will make the robot travel ahead for a certain distance (one square on the map). To use it, just drag the block in the drop area, press \"Play\", and you will see the robot moving! After the instruction the is executed, the mars explorer will stop.\n\nAs you can see, the robot is going to do exactly (!) what you tell him: awesome, isn't it? Now, say, you want him to move a bit more than one square. What you need to do is to just add one (or more) \"moveForward\" blocks one after another, and the instruction will be executed several times.\n\nDid you know?\n\nIn programming an equivalent of the instruction is called a function - some action, that programmer tells computer to execute, for instance \"display a line of text\". Functions are a core component of any software. Your favourite video game or website - they all have functions in their core. In our game, the robot is your computer, which you, like a real software developer, program!\n\nLevel 1b\n\nDo you remember, that if you put one instruction \"moveForward\", it would make the robot move exactly one cell forward? Then, if you would like him to travel a longer distance, you could make the sequence of instructions:  \"moveForward\", \"moveForward\",  \"moveForward\"…\n\nThat works alright if you know exactly which distance you need to pass - but what if you are not sure? Well, don’t worry, we got it all covered: let me present the \"while\" loop!\n\n“While loop? What is that? That sounds strange!“.\n\nIf you want the explorer to travel some distance until your \"X\" target, aren't you telling the robot something like \"move while you can move, just go ahead!\". The robot is, actually, quite clever, so it can understand that, and make things easier for you: you just have to put together blocks:  “while\"  \"can_moveForward\", \"moveForward”. As simple as that! Then, the explorer will be going straight until he faces an obstacle. \n\nDid you know?\n\nLoop is another fundamental programming concept - it is commonly used to perform repetitive tasks. There are several types of them, but in our game you will only have to deal with \"while\". Conditions always go together with loops. They are specific statements that determine when a loop will stop. For instance in \"while\" loop, it runs until its condition becomes false (for instance 1 > 2 is false). ");
		}

		if (tutorial_to_display == 2) {
			GUI.Label (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width*0.49f), "So far we just had one instruction - \"moveForward\". Can a robot be really useful, if it can only move forward? That would have been be a pretty useless robot, right? \n\nNow imagine that you have new instructions, so your robot can do new things! Do you see where this is leading to? We can use more and more instructions, so the robot can have a more interesting behaviour!\n\nOn Level 2, the new instructions we have are pretty cool: now, the robot can actually turn clockwise and anti-clockwise (pretty useful, when trying to travel somewhere!)\n\nDid you know ?\n\nIn the real world, those instructions (functions) are put together, in something programmers call a “library” - a collection of functions. They can later be reused in new applications. That saves a lot of time - you don't need to write the same code anymore!\n\nOh, and by the way, more cool features are coming in the next levels, just wait for it!\n\n");	
		}

		if (tutorial_to_display == 3) {
			GUI.Label (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width*0.49f), "Do you remember, when I told you that the more blocks you have, the more your robot can do? Well, let me show you a brand new one: “notReached_target”. You might wonder “this is not even an instruction, it doesn’t do anything!”. You’re completely right: it doesn’t do anything on its own. But do you remember the “while” block? Well, if you put those two together, you can ask your robot to “do something, as long as we are not at the target”. “notReached_target” is a condition, exactly same type of block as \"can_moveForward\".\n\nNow, you might wonder - so how can it be useful for me? It’s simple: it will make your robot perform some action until the end of level. The best way to explore it is to just play with it! I’m sure you can solve this puzzle with this new instruction!\n\nDid you know ?\n\nSometimes, even such clever guys as programmers are stuck with their code - they just don’t know how to make it work.. but they never give up! So, if you face difficulties, don't be afraid to play around with instructions, and just see what happens! We are sure, you are clever enough to solve all our puzzles!");	
		}

		if (tutorial_to_display == 4) {
			GUI.Label (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width*0.49f), "Oh my God, now there are obstacles on the map! The robot definitely doesn't feel like crushing into those! But don’t forget, it cannot avoid the obstacles without your help, you have to tell him how to do that!\n\nMaybe you didn’t notice yet, but you can actually place blocks inside other constructions of blocks. This will help to solve complicated problems in an easy way!\n\nFor example, you can actually put loops into loops. Yeah, you heard me right, you can actually do that! Now you can solve very difficult levels with this super trick, but don’t forget: If you’re stuck, just try another approach, even if it might seem ridiculous at first! \n\nDid you know ?\n\nDid you ever want to have a real robot, that could be able to teach to do awesome things, just like in the movies? To be honest, I did have such experience! But let me tell you that actually, controlling the robot in this game is very similar to controlling a real one! Yes, it is true: people that are programming robots, drones and any other hardware in the real world are actually using the same principles as you are in our game: they use functions, loops, conditions, put them together, test their robot, and have fun! Fantastic, isn’t it?");	
		}

		if (tutorial_to_display == 5) {
			GUI.Label (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width*0.49f), "Whoah, you made a pretty big progress, congratulations!\n\nUp until now, you didn’t really need to count the tiles to solve the puzzles. But hey, sometimes you do! (spoiler alert: in this level, you will have to count some tiles)\n\nYou will see that in some levels, counting is really useful. “But how can I count, the tiles are all really similar”. But no worries, we got you covered: looking at the way decorations and obstacles are positioned you can help you! Also, you know that one \"moveForward\" function will move your character by exactly one tile ahead - you can first launch the robot with it to see how far he moves.\n\nDid you know?\n\nWhen you will be programming real robots one day, you will see that actually, measuring distance is really useful. Have you ever seen one of those amazing robot vacuum cleaners? You might be wondering:  “How do they avoid bumping into my chair, my table and other objects?”. What they do is the same sort of thing as will be doing: they “count” how far they are from your chair, and when they’re too close, they change direction!");	
		}

		if (tutorial_to_display == 6) {
			GUI.Label (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width*0.56f), "\"Wait a second.. this looks very similar, I already saw this level before.. I think…”\n\nYes, it’s true, you actually already solved a very similar problem (for the curious, it was Level 3). But still, something looks different, right? Well now, we made it a little bit more sophisticate, we added more obstacles! We thought - if he or she is brainy enough to get that far in our game, that should not be a problem!\n\nThere is a funny thing about programmers: sometimes, when they are faced with a problem, they can clearly recall that they already solved one, which was very similar. If you think about that, you could just take the solution you used before, and some new features to it! Try it, and see if that works for you.\n\nDid you know?\n\nDo you remember when I told you about libraries? Now, just imagine, for a moment, that you could save your solution for Level 3 somewhere, and just take that series of blocks when you need it, so you don’t need to rewrite it again. Sounds very useful, right? Well, surprise - that’s actually how programmers do their code: once they solve a problem, they keep their solution, and use it to deal with more complicated problems. Then, they save the new problem, and use it for an even of an even more difficult puzzle. Do you see what’s happening there: at the end, they can use experience of each other to do super cool things in a very short time!");	
		}

		if (tutorial_to_display == 7) {
			GUI.Label (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width*0.72f), "So, we saw how you can do something repeatedly with “while” block, but sometimes, that’s just not enough to solve a problem. Did you try going into these new lava obstacle? Go on, try it out, I’ll wait here!\n\nSo, how did it go?  Not that good, right? It told you!\n\nNow, you need a new instruction in your toolkit: the “if” statement. “What is that?”, you will ask me. This is an essential part of almost any computer program, so listen carefully. Also, it is very easy to understand. I will give you an example for real life:\n\nif  *I’m hungry*\n\tI eat\n\nAnother one:  \n\nIf *I’m bored*\n\tI play with my friends\n\nEasy, right? I'm pretty sure now you can come up with your own examples, it’s pretty funny!\n\nDid you know?\n\nThe \"if\" statements, which are already very useful on their own, can be even more helpful if combined with while loops. A lot can be done using this powerful combination! \nGradually, you are learning more and more about programming. At this stage, you can have a proper conversation with a programmer about things he uses in code - he’ll instantly understand what you’re talking about! We are not that far away from also making you a proper programmer!\n\nI suggest you now you to do different things with that new instruction - that always the best way to learn something in programming! Just see what happens!");	
		}

		if (tutorial_to_display == 8) {
			GUI.Label (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width*0.45f), "Hey, you went really, really far! We didn't expect you to do it so fast! You should be extremely clever! \n\nNow, as this game is too easy for you, we are going to make things a little bit more complicated: some blocks are not available anymore, and you have to solve this puzzle only with what is given to you!\n\nDid you know?\n\nSometimes, programmes are in the same situation as you are: some code elements, for whatever reason, becomes unavailable, and they have to solve their \"puzzles\" with a smaller toolkit! A common example is when a computer, which is being programmed, does not have enough memory to support complicated functions. So, if you solve this puzzle with what you have, you can proudly say, that you have faced the same challenges as real programmers do!");	
		}

		if (tutorial_to_display == 9) {
			GUI.Label (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width * 0.9f), "Hey, hey, hey you went that far !! \nDo you remember the “if” statement ?\nA little refresh:\nSay you have to do some homework (boring, right ??)\nYou would write something like:\nif I didn’t finish my homework\n  Do some stuff :p\nNow, say you did do your homework. How do you express that ?\nOk, so you’re used to this, but I’m gonna introduce a new instruction: the “else\" statement.\nYou just write it after the “if” statement:\nif I didn’t finish my homework\n  Do some stuff :p\nelse\n  Go play !!\nCool, isn’t it ?\nNow you can solve this problem with that super new instruction !\n\n\nDid you know ?\nOk, so now, you’re gonna learn a pretty cool word you can boast with: Algorithm.\n“Oooh that sounds like a scary word”. Yes, I agree, it does. But bear with me, it’s actually super cool, and you actually use algorithms everyday in your life without knowing it !\nSay I want you to cook some pastas.\nTypically, what you’re gonna do is:\nChoose your pasta\nBoil some water\nAdd pasta to the boiling water\nFilter the water\nEnjoy!!\n\nNow you might wonder, “why are there pastas here? What does this have to do with “Algorithm” ?? \"\nWell, what I just described is actually: a series of steps, that you take in a certain order, to have something you want at the end (here the result is some pastas :p).\nHold and behold, this is actually the definition of an algorithm! It’s as simple as that !!\nNow you might think “But then, everything I did in this game was designing algorithms to solve a puzzle”. You’re absolutely correct. That’s exactly it !!\nAnd that’s what developers do: they design algorithms so that you can watch youtube videos, play some games, or browse the Internet !\n");
		}

		if (tutorial_to_display == 10) {
			GUI.Label (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width *0.65f), "Ok, now this is serious stuff!\nYou might have noticed that you now have -a lot- of instructions available, and you have a lot of choices of path for the robot to take.. Yeah ! Up until now, all the levels had one solution only (on some cases you could do two very similar solutions though :p).\nNow this level is special: you can solve it in many, many, different ways. There’s no “bad” solution. So now, you can actually experiment with all instructions as much as you want to see what happens!\nHere’s a challenge for the most curious ones: Once you find one solution that works, try to tackle the puzzle in a different way, and try to see if you can come up with a different idea. Then just have a look at both solutions: which one is faster? simpler? longer? You can also compare your solution to your friend’s ideas. \n\n\nDid you know?\nIn the real world, that is how things actually work! There is no “single” solution to a problem. In fact, if you put, say 100 developers in a test, asking them to solve whatever problem you come up with, you’ll see 100 different solutions !! But the most amazing part of this is that it is possible to have a 100 different solutions to a problem that all work !! That is what programming is also about: you can always come up with a different solution than your friend, and you see how free you are to experiment with robots/apps/websites/games…\n\nOne last thing too: You may have noticed that some solutions are better, shorter and/or simpler than others. We say that some solutions are more efficient than others. In their job, developers always try to be more “efficient”: they always try to have a super cool solution to their super cool problems!");
		}

		if (tutorial_to_display == 11) {
			GUI.Label (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width *2f)," 1.moveForward;\nmoveForward;\nmoveForward;\n\n\n1B. \nwhile (canMove) \n{ \nmoveForward;\n }\n\n\n2. \n while (canMove)\n{ \nmoveForward; \n}\nturnRight;\nwhile (canMove)\n{ \nmoveForward; \n}\n\n\n3. \nwhile (notReached_target) { \nwhile (canMove) {\nmoveForward;\n} \nturnLeft;\n}\n\n\n4.\nwhile (notReached_target) { \nwhile (canMove) {\nmoveForward;\n} \nturnLeft;\n}\n\n\n5.\nwhile (notReached_target) { \nmoveForward;\n}\n\n\n6.\nwhile (notReached_target) { \nwhile (canMove) {\nmoveForward;\n} \nturnLeft;\n}\n\n\n7.\nwhile (notReached_target) { \nif (not_canMove) {\nturnLeft;\n}\nmoveForward;\n}\n\n\n8.\nwhile (canMove) \n{ \nmoveForward; \n}\nturnLeft;\nmoveForward;\nturnLeft;\nmoveForward;\nturnLeft;\nwhile (canMove) \n{ \nmoveForward; \n}\n\n9.\nwhile (notReached_target) {\nmoveForward;\nif (noObstacles_around) {\nturnLeft;\n} else {\nturnRight;\nturnRight;\n}\n}\n\n10.\nwhile (notReached_target) { \nwhile (canMove) {\nmoveForward;\n} \nturnLeft;\n}\n");
		}

		GUI.EndScrollView();


		if (GUI.Button (new Rect (Screen.width*0.003f, Screen.height*0.8f, button_width, button_height), "Main menu",menu_button_style)) {
			SceneManager.LoadScene ("main_menu");
		}


	}

}
