---
title: Development (dev) - Script
hide:
---

# Development (dev) - Script
[Quest Index](./index.md) - Language: english - [french](./dev-script.fr.md) - [polish](./dev-script.pl.md) - [italian](./dev-script.it.md)

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// DEV QUEST SCRIPT</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// Country: Italy - Firenze</span>
<span class="yarn-header-dim">// Content: Living document for testing Yarn features</span>
<span class="yarn-header-dim">// and documenting available commands</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// INIT</span>
<span class="yarn-header-dim">// every quest starts with an INIT node</span>
<span class="yarn-header-dim">// to inizialize existing variables and declare new ones</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// title of a node must be unique to this quest</span>
<span class="yarn-header-dim">// create a new node for anything that must be referenced multi times</span>
<span class="yarn-header-dim">// that makes understanding of the flow easier</span>
<span class="yarn-header-dim">// position is used to place the node in the Graph view in the Editor</span>
<span class="yarn-header-dim">// group is used to organize the nodes in the Graph view in the Editor</span>
<span class="yarn-header-dim">group: docs</span>
<span class="yarn-header-dim">// tags are used to add metadata to the node</span>
<span class="yarn-header-dim">// - actor is the speacking character, used mostly for voice overs</span>
<span class="yarn-header-dim">// - type is used to identify special nodes like Start, End, Quiz</span>
<span class="yarn-header-dim">tags: actor=Guide, type=Quiz</span>
<span class="yarn-header-dim">// color us used like this:</span>
<span class="yarn-header-dim">//  - red for important nodes like init and quest end</span>
<span class="yarn-header-dim">//  - blue for NPCs</span>
<span class="yarn-header-dim">//  - yellow for items</span>
<span class="yarn-header-dim">//  - green for tasks</span>
<span class="yarn-header-dim">//  - purple for activities</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// these are the common variables</span>
<span class="yarn-cmd">&lt;&lt;set $EASY_MODE = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $IS_DESKTOP = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>

<span class="yarn-comment">// here we declare new variables for this quest</span>
<span class="yarn-cmd">&lt;&lt;declare $met_teacher = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_hint = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $doorUnlocked = false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-docs-commands"></a>
## docs_commands

<div class="yarn-node" data-title="docs_commands"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// COMMANDS</span>
<span class="yarn-header-dim">// these are the commands available in the game</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: docs</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>

<span class="yarn-comment">// execute the configured action in the actionManager</span>
<span class="yarn-cmd">&lt;&lt;action action_id&gt;&gt;</span>

<span class="yarn-comment">// camera commands</span>
<span class="yarn-cmd">&lt;&lt;camera_focus palace&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;card fr_baguette show&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card_hide&gt;&gt;</span>
<span class="yarn-comment">// shows the image</span>
<span class="yarn-cmd">&lt;&lt;asset id_asset&gt;&gt;</span> 
<span class="yarn-comment">// hides the current image</span>
<span class="yarn-cmd">&lt;&lt;asset_hide&gt;&gt;</span>

<span class="yarn-comment">// adds the item = card to the inventory (needs to be collectable!)</span>
<span class="yarn-cmd">&lt;&lt;inventory id_card add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory id_card&gt;&gt;</span> // same as add
<span class="yarn-cmd">&lt;&lt;inventory id_card remove&gt;&gt;</span>

<span class="yarn-comment">// starts a task</span>
<span class="yarn-cmd">&lt;&lt;task_start id_task_configuration&gt;&gt;</span>
<span class="yarn-comment">// end a task... if you say nothing, its a success</span>
<span class="yarn-cmd">&lt;&lt;task_end id_task_configuration&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_end id_task_configuration fail&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_end&gt;&gt;</span> // or even without the id to end current task


<span class="yarn-comment">// launches an activity</span>
<span class="yarn-cmd">&lt;&lt;activity id_activity_configuration&gt;&gt;</span>

<span class="yarn-comment">// end current quest and calculate stars based on progress</span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>
<span class="yarn-comment">// end the current quest with n (0, 1,2,3) stars</span>
<span class="yarn-cmd">&lt;&lt;quest_end 3&gt;&gt;</span>

<span class="yarn-comment">// --- Yarn built-in commands ---</span>
<span class="yarn-cmd">&lt;&lt;wait 3&gt;&gt;</span> // waits for 3 seconds
<span class="yarn-cmd">&lt;&lt;stop&gt;&gt;</span> // stops the current dialogue&gt;&gt;


</code></pre></div>

<a id="ys-node-docs-functions"></a>
## docs_functions

<div class="yarn-node" data-title="docs_functions"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// FUNCTIONS</span>
<span class="yarn-header-dim">// these are the functions available in the game</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">group: docs</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// these are the functions available in the game</span>

&lt;&lt;if item_count("flag_france") &gt;= 2&gt;&gt;
<span class="yarn-line">    You have more than 2 Flags of France <span class="yarn-meta">#line:08a1058 #native</span></span>
<span class="yarn-line">    another line <span class="yarn-meta">#line:0563122 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;if has_item("flag_france")&gt;&gt;</span>
<span class="yarn-line">    You have a Flag of France <span class="yarn-meta">#line:kidm_0b932d0 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;if has_item_at_least("flag_france", 3)&gt;&gt;</span>
<span class="yarn-line">    You have at least 3 Flags of France <span class="yarn-meta">#line:0d694f1 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;if can_collect("flag_france")&gt;&gt;</span>
<span class="yarn-line">    You can collect a Flag of France <span class="yarn-meta">#line:03da430 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-test-inventory"></a>
## test_inventory

<div class="yarn-node" data-title="test_inventory"><pre class="yarn-code"><code><span class="yarn-header-dim">group: inventory</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I am hungry. how about some food? <span class="yarn-meta">#line:0bdf566 </span></span>
<span class="yarn-cmd">&lt;&lt;if has_item_at_least("fr_baguette", 3)&gt;&gt;</span>
<span class="yarn-line">    I need 3 baguettes! <span class="yarn-meta">#line:00c341e </span></span>
    <span class="yarn-cmd">&lt;&lt;inventory fr_baguette remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory fr_baguette remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory fr_baguette remove&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    You don't have enough baguettes! <span class="yarn-meta">#line:0c0d786 </span></span>
    <span class="yarn-cmd">&lt;&lt;if has_item("fr_fish")&gt;&gt;</span>
<span class="yarn-line">        But you have some fish! <span class="yarn-meta">#line:06c9370 </span></span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">    Thank you. <span class="yarn-meta">#line:0a7921b </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-got-baguette"></a>
## got_baguette

<div class="yarn-node" data-title="got_baguette"><pre class="yarn-code"><code><span class="yarn-header-dim">group: inventory</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You found a baguette! <span class="yarn-meta">#line:dev_got_baguette </span></span>
<span class="yarn-cmd">&lt;&lt;inventory fr_baguette add&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-got-fish"></a>
## got_fish

<div class="yarn-node" data-title="got_fish"><pre class="yarn-code"><code><span class="yarn-header-dim">group: inventory</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">An healthy fish! <span class="yarn-meta">#line:dev_got_fish </span></span>
<span class="yarn-cmd">&lt;&lt;inventory fr_fish add&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-test-cards"></a>
## test_cards

<div class="yarn-node" data-title="test_cards"><pre class="yarn-code"><code><span class="yarn-header-dim">group: cards </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Now i show you some cards <span class="yarn-meta">#line:05edfca </span></span>
<span class="yarn-cmd">&lt;&lt;card fr_fish&gt;&gt;</span>
<span class="yarn-line">Please look at its details <span class="yarn-meta">#line:0adbe4f </span></span>
<span class="yarn-cmd">&lt;&lt;card fr_fish zoom&gt;&gt;</span>
<span class="yarn-line">go back to the menu and browse the cards <span class="yarn-meta">#line:017a620 </span></span>
<span class="yarn-line">and now... <span class="yarn-meta">#line:04ade19 </span></span>
<span class="yarn-cmd">&lt;&lt;card fr_baguette&gt;&gt;</span>
<span class="yarn-line">Good, isnt' it? <span class="yarn-meta">#line:04b02ce </span></span>
<span class="yarn-cmd">&lt;&lt;card_hide&gt;&gt;</span>
<span class="yarn-line">GO back to work now. <span class="yarn-meta">#line:0970ed1 </span></span>


</code></pre></div>

<a id="ys-node-test-camera"></a>
## test_camera

<div class="yarn-node" data-title="test_camera"><pre class="yarn-code"><code><span class="yarn-header-dim">group: camera </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Now i show you the map of the playgroud <span class="yarn-meta">#line:0c58bd8 </span></span>
<span class="yarn-cmd">&lt;&lt;camera_focus map_tutorial&gt;&gt;</span>
<span class="yarn-line">Do you see that point? <span class="yarn-meta">#line:03e0224 </span></span>
<span class="yarn-cmd">&lt;&lt;camera_focus map_tutorial_detail&gt;&gt;</span>
<span class="yarn-line">Now Look at that palace! <span class="yarn-meta">#line:0c1b2e4 </span></span>
<span class="yarn-cmd">&lt;&lt;camera_focus palace&gt;&gt;</span>
<span class="yarn-line">Nice isnt' it? <span class="yarn-meta">#line:04daace </span></span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-test-actions"></a>
## test_actions

<div class="yarn-node" data-title="test_actions"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=KID_FEMALE</span>
<span class="yarn-header-dim">group: actions</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action activate_elevator&gt;&gt;</span>
<span class="yarn-line">Action Pre "pre_actions" and action_post "post_actions" <span class="yarn-meta">#line:0a67b25 </span></span>
<span class="yarn-line">Do you want to open the Chest? <span class="yarn-meta">#line:03cae39 </span></span>
<span class="yarn-line">-&gt; Yes <span class="yarn-meta">#line:067231b </span></span>
    <span class="yarn-cmd">&lt;&lt;action open_chest&gt;&gt;</span>
<span class="yarn-line">-&gt; No <span class="yarn-meta">#line:05d9657 </span></span>
<span class="yarn-cmd">&lt;&lt;action stop_elevator&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-collect-apples"></a>
## task_collect_apples

<div class="yarn-node" data-title="task_collect_apples"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">group: tasks</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("collect_apples")&gt;&gt;</span>
<span class="yarn-line">    Thank you! you finished this task <span class="yarn-meta">#line:dev_task_collect_apples_2</span></span>
&lt;&lt;elseif GetCollectedItem("collect_apples") &gt; 0 &gt;&gt;
<span class="yarn-line">    I need more apples! <span class="yarn-meta">#line:dev_task_collect_apples_3</span></span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Collect 4 apples! <span class="yarn-meta">#line:dev_task_collect_apples_1 </span></span>
    <span class="yarn-cmd">&lt;&lt;task_start collect_apples task_collect_apples_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-collect-apples-done"></a>
## task_collect_apples_done

<div class="yarn-node" data-title="task_collect_apples_done"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">group: tasks</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Well done! Go back to the robot <span class="yarn-meta">#line:dev_test_task_done_1 </span></span>

</code></pre></div>

<a id="ys-node-task-open-chest"></a>
## task_open_chest

<div class="yarn-node" data-title="task_open_chest"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">group: tasks</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("open_chest")&gt;&gt;</span>
<span class="yarn-line">    Thanks you... I needed that</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Reach and open that chest! </span>
    <span class="yarn-cmd">&lt;&lt;task_start open_chest task_open_chest_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-open-chest-done"></a>
## task_open_chest_done

<div class="yarn-node" data-title="task_open_chest_done"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">group: tasks</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Well done! You opened the chest. </span>

</code></pre></div>

<a id="ys-node-item-chest"></a>
## item_chest

<div class="yarn-node" data-title="item_chest"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">group: actions</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">A good chest! <span class="yarn-meta">#line:dev_item_chest_1 </span></span>
<span class="yarn-cmd">&lt;&lt;action open_chest&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-test-quiz"></a>
## test_quiz

<div class="yarn-node" data-title="test_quiz"><pre class="yarn-code"><code><span class="yarn-header-dim">group: quiz</span>
<span class="yarn-header-dim">tags: type=quiz</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Calculate 3*2 <span class="yarn-meta">#line:dev_test_quiz_1 </span></span>
<span class="yarn-line">-&gt; 4 <span class="yarn-meta">#line:dev_test_quiz_2 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump test_quiz_wrong&gt;&gt;</span>
<span class="yarn-line">-&gt; 6 <span class="yarn-meta">#line:dev_test_quiz_3 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump test_quiz_correct&gt;&gt;</span>
<span class="yarn-line">-&gt; 8 <span class="yarn-meta">#line:dev_test_quiz_4 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump test_quiz_wrong&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-test-quiz-correct"></a>
## test_quiz_correct

<div class="yarn-node" data-title="test_quiz_correct"><pre class="yarn-code"><code><span class="yarn-header-dim">group: quiz</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-line">YES! Correct. <span class="yarn-meta">#line:0eb010e </span></span>

</code></pre></div>

<a id="ys-node-test-quiz-wrong"></a>
## test_quiz_wrong

<div class="yarn-node" data-title="test_quiz_wrong"><pre class="yarn-code"><code><span class="yarn-header-dim">group: quiz</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-line">GUIDE: No. Do you want to try again? <span class="yarn-meta">#line:0e2b8d7 </span></span>
<span class="yarn-line">-&gt; Yes <span class="yarn-meta">#line:08ff2b9 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump test_quiz&gt;&gt;</span>
<span class="yarn-line">-&gt; No <span class="yarn-meta">#line:042881c </span></span>

</code></pre></div>

<a id="ys-node-endgame"></a>
## endgame

<div class="yarn-node" data-title="endgame"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">group: endgame</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>

<span class="yarn-line">You can end this quest with... <span class="yarn-meta">#line:027bdca </span></span>
<span class="yarn-line">-&gt; Zero stars (fail) <span class="yarn-meta">#line:0130629 </span></span>
    <span class="yarn-cmd">&lt;&lt;quest_end 0&gt;&gt;</span>
<span class="yarn-line">-&gt; 1 star <span class="yarn-meta">#line:05caf8d </span></span>
    <span class="yarn-cmd">&lt;&lt;quest_end 1&gt;&gt;</span>
<span class="yarn-line">-&gt; 2 stars <span class="yarn-meta">#line:02466da </span></span>
    <span class="yarn-cmd">&lt;&lt;quest_end 2&gt;&gt;</span>
<span class="yarn-line">-&gt; 3 stars <span class="yarn-meta">#line:0f65a6b </span></span>
    <span class="yarn-cmd">&lt;&lt;quest_end 3&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-npcgioconda"></a>
## NPCGIoconda

<div class="yarn-node" data-title="NPCGIoconda"><pre class="yarn-code"><code><span class="yarn-header-dim">assetimage: antura_hero</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset food_tomato&gt;&gt;</span>
<span class="yarn-line">ciao, stefano <span class="yarn-meta">#line:0c0329c </span></span>
<span class="yarn-line">ti piacciono le foto? <span class="yarn-meta">#line:0b9b60d </span></span>
<span class="yarn-cmd">&lt;&lt;asset food_fish&gt;&gt;</span>
<span class="yarn-line">-&gt; vuoi arancia? <span class="yarn-meta">#line:0c2bd41 </span></span>
    <span class="yarn-cmd">&lt;&lt;asset food_orange&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action play_sfx&gt;&gt;</span>
<span class="yarn-line">-&gt; vuoi olio? <span class="yarn-meta">#line:034ed48 </span></span>
    <span class="yarn-cmd">&lt;&lt;asset food_oil&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action play_sfx&gt;&gt;</span>
<span class="yarn-line">ora chiudo <span class="yarn-meta">#line:0baeb5f </span></span>
<span class="yarn-cmd">&lt;&lt;asset_hide&gt;&gt;</span>
<span class="yarn-line">-&gt; Activity Order <span class="yarn-meta">#line:0087ab1 </span></span>
    <span class="yarn-cmd">&lt;&lt;activity activity_test&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump activity_done&gt;&gt;</span>
<span class="yarn-line">-&gt; Suona sfx <span class="yarn-meta">#line:0f02889 </span></span>
    <span class="yarn-cmd">&lt;&lt;action play_sfx&gt;&gt;</span>
<span class="yarn-line">-&gt; Apri Baule <span class="yarn-meta">#line:053f800 </span></span>
    <span class="yarn-cmd">&lt;&lt;action open_chest&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-activity-done"></a>
## activity_done

<div class="yarn-node" data-title="activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">---</span>
<span class="yarn-line">Activity finished. <span class="yarn-meta">#line:0ce187d </span></span>

</code></pre></div>

<a id="ys-node-npcgreeting2"></a>
## NPCGreeting2

<div class="yarn-node" data-title="NPCGreeting2"><pre class="yarn-code"><code><span class="yarn-header-dim">assetimage: pirates</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-comment">// <span class="yarn-cmd">&lt;&lt;activity activity_memory&gt;&gt;</span></span>
<span class="yarn-line">Narrator: Hallo <span class="yarn-meta">#line:08ecb6c </span></span>
<span class="yarn-line">Narrator: How are you? <span class="yarn-meta">#line:00b7839 </span></span>
<span class="yarn-line">-&gt; Good <span class="yarn-meta">#line:0991140 </span></span>
<span class="yarn-line">    Narrator: Good to hear that <span class="yarn-meta">#line:0b3477e </span></span>
<span class="yarn-line">-&gt; Bad <span class="yarn-meta">#line:08607a4 </span></span>
<span class="yarn-line">    Narrator: Oh, I am sorry to hear that <span class="yarn-meta">#line:0e6ecb2 </span></span>
<span class="yarn-line">-&gt; Neutral <span class="yarn-meta">#line:05f3ea5 </span></span>
<span class="yarn-line">    Narrator: I see, I hope it gets better soon <span class="yarn-meta">#line:0802baf </span></span>
<span class="yarn-line">Narrator: What do you want to do with  coins? <span class="yarn-meta">#line:0bcd506 </span></span>

</code></pre></div>

<a id="ys-node-npcgreeting3"></a>
## NPCGreeting3

<div class="yarn-node" data-title="NPCGreeting3"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: #camera2 background:conductor_cabin</span>
<span class="yarn-header-dim">---</span>

<span class="yarn-cmd">&lt;&lt;declare $coins = 0&gt;&gt;</span>
<span class="yarn-line">Narrator: What do you want to do with {$coins} coins?  <span class="yarn-meta">#line:0a1994c </span></span>
<span class="yarn-line">-&gt; Open the door <span class="yarn-meta">#line:0e11bee </span></span>
 <span class="yarn-cmd">&lt;&lt;set $doorUnlocked = true&gt;&gt;</span>
<span class="yarn-line"> Narrator: The door is open <span class="yarn-meta">#line:0cac01d </span></span>
<span class="yarn-line">-&gt; Stay at home <span class="yarn-meta">#line:07f4239 </span></span>
<span class="yarn-line"> Narrator: You stay at home <span class="yarn-meta">#line:079b16e </span></span>
 <span class="yarn-cmd">&lt;&lt;set $coins = $coins + 10&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-chest-01"></a>
## Chest_01

<div class="yarn-node" data-title="Chest_01"><pre class="yarn-code"><code><span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $doorUnlocked&gt;&gt;</span>
<span class="yarn-line">Narrator: la porta è aperta <span class="yarn-meta">#line:0e22a89 </span></span>
<span class="yarn-cmd">&lt;&lt;jump EntroInCasa&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Narrator: la porta è chiusa <span class="yarn-meta">#line:05086bc </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-note-for-the-editors"></a>
## note_for_the_editors

<div class="yarn-node" data-title="note_for_the_editors"><pre class="yarn-code"><code><span class="yarn-header-dim">style: note</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Custom note <span class="yarn-meta">#line:04d621e </span></span>
<span class="yarn-line">this is useful to leave notes for the editors <span class="yarn-meta">#line:0520fb4 </span></span>
<span class="yarn-line">like reminders of things to do <span class="yarn-meta">#line:078fe99 </span></span>
<span class="yarn-line">or things to check <span class="yarn-meta">#line:0982259 </span></span>
<span class="yarn-line">or things to fix <span class="yarn-meta">#line:096379b </span></span>

</code></pre></div>

<a id="ys-node-entroincasa"></a>
## EntroInCasa

<div class="yarn-node" data-title="EntroInCasa"><pre class="yarn-code"><code><span class="yarn-header-dim">---</span>
<span class="yarn-line">entro in casa <span class="yarn-meta">#line:0f92df5 </span></span>


</code></pre></div>

<a id="ys-node-coin-machine"></a>
## coin_machine

<div class="yarn-node" data-title="coin_machine"><pre class="yarn-code"><code><span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">group: variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I'll give you 100 coins. <span class="yarn-meta">#line:06427f9 </span></span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = $TOTAL_COINS + 100&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-test-mood"></a>
## test_mood

<div class="yarn-node" data-title="test_mood"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=GUIDE, mood=HAPPY</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Happy Mood <span class="yarn-meta">#line:0a6e648 </span></span>


</code></pre></div>

<a id="ys-node-random-lines"></a>
## random_lines

<div class="yarn-node" data-title="random_lines"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">group: random</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Smart random Line 1 <span class="yarn-meta">#line:07193d8 </span></span>
<span class="yarn-line">Smart random Line 2 <span class="yarn-meta">#line:06fe9a7 </span></span>
<span class="yarn-line">Smart random Line 3 <span class="yarn-meta">#line:032e777 </span></span>

</code></pre></div>

<a id="ys-node-multiple-lines"></a>
## multiple_lines

<div class="yarn-node" data-title="multiple_lines"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=KID_FEMALE</span>
<span class="yarn-header-dim">group: random</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">=&gt; This is Hint 1 <span class="yarn-meta">#line:0423295 </span></span>
<span class="yarn-line">=&gt; This is Hint 2 <span class="yarn-meta">#line:00d69e5 </span></span>
<span class="yarn-line">=&gt; Hint 3 <span class="yarn-meta">#line:08966ba </span></span>


</code></pre></div>

<a id="ys-node-activity-canvas"></a>
## activity_canvas

<div class="yarn-node" data-title="activity_canvas"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Activity CANVAS! </span>
<span class="yarn-cmd">&lt;&lt;activity activity_canvas activity_canvas_result&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-canvas-result"></a>
## activity_canvas_result

<div class="yarn-node" data-title="activity_canvas_result"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("activity_canvas") &gt; 0&gt;&gt;
<span class="yarn-line">You played CANVAS activity well!</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Try again.</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-jigsaw"></a>
## activity_jigsaw

<div class="yarn-node" data-title="activity_jigsaw"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Activity JIGSAW PUZZLE! </span>
<span class="yarn-cmd">&lt;&lt;activity activity_jigsaw activity_jigsaw_result&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-jigsaw-result"></a>
## activity_jigsaw_result

<div class="yarn-node" data-title="activity_jigsaw_result"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("activity_jigsaw") &gt; 0&gt;&gt;
<span class="yarn-line">You played JIGSAW PUZZLE activity well!</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Try again.</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-memory"></a>
## activity_memory

<div class="yarn-node" data-title="activity_memory"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Activity MEMORY! </span>
<span class="yarn-cmd">&lt;&lt;activity activity_memory activity_memory_result&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-memory-result"></a>
## activity_memory_result

<div class="yarn-node" data-title="activity_memory_result"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("activity_memory") &gt; 0&gt;&gt;
<span class="yarn-line">You played MEMORY activity well!</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Try again.</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-match"></a>
## activity_match

<div class="yarn-node" data-title="activity_match"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Activity MATCH! </span>
<span class="yarn-cmd">&lt;&lt;activity activity_match activity_match_result&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-match-result"></a>
## activity_match_result

<div class="yarn-node" data-title="activity_match_result"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("activity_match") &gt; 0&gt;&gt;
<span class="yarn-line">You played MATCH activity well!</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Try again.</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-money"></a>
## activity_money

<div class="yarn-node" data-title="activity_money"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Activity MONEY! </span>
<span class="yarn-cmd">&lt;&lt;activity activity_money activity_money_result&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-money-result"></a>
## activity_money_result

<div class="yarn-node" data-title="activity_money_result"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("activity_money") &gt; 0&gt;&gt;
<span class="yarn-line">You played MONEY activity well!</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Try again.</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-order"></a>
## activity_order

<div class="yarn-node" data-title="activity_order"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Activity ORDER! </span>
<span class="yarn-cmd">&lt;&lt;activity activity_order activity_order_result&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-order-result"></a>
## activity_order_result

<div class="yarn-node" data-title="activity_order_result"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("activity_order") &gt; 0&gt;&gt;
<span class="yarn-line">You played ORDER activity well!</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Try again.</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-piano"></a>
## activity_piano

<div class="yarn-node" data-title="activity_piano"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=KID_FEMALE</span>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Play this PIANO Melody! <span class="yarn-meta">#line:0e11b77 </span></span>
<span class="yarn-cmd">&lt;&lt;activity activity_piano activity_piano_result&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-piano-result"></a>
## activity_piano_result

<div class="yarn-node" data-title="activity_piano_result"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=KID_FEMALE</span>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("activity_piano") &gt; 0&gt;&gt;
<span class="yarn-line">You played the PIANO activity well!</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Try again.</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>


