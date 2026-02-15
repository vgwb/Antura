---
title: Paris Seine (fr_10) - Script
hide:
---

# Paris Seine (fr_10) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-10-paris-seine/29/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=754141150#gid=754141150)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_10%20Paris%20Seine/FR_10%20Paris%20Seine%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_10 | Seine (Paris)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $boat_goods = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $boat_people = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $boat_house = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $bridge_alexandre = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $bridge_train = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $bridge_cars = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $bridge_people = 0&gt;&gt;</span>
<span class="yarn-line">We are at the River Seine, in Paris!</span> <span class="yarn-meta">#line:042160f </span>
<span class="yarn-line">Today we will explore the river, the bridges, and the boats.</span> <span class="yarn-meta">#line:0280e8f </span>
<span class="yarn-cmd">&lt;&lt;target protagonist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_init&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;area area_all&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great, you finished the quest!</span> <span class="yarn-meta">#line:0c408c3 </span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_people&gt;&gt;</span>
<span class="yarn-line">You learned about bridges for trains, cars, and people.</span> <span class="yarn-meta">#line:091e9fc </span>
<span class="yarn-cmd">&lt;&lt;card boat_people&gt;&gt;</span>
<span class="yarn-line">You saw tourist boats with big windows.</span> <span class="yarn-meta">#line:062cc35 </span>
<span class="yarn-cmd">&lt;&lt;card boat_house&gt;&gt;</span>
<span class="yarn-line">You also saw house boats where people can live.</span> <span class="yarn-meta">#line:0a29742 </span>
<span class="yarn-cmd">&lt;&lt;card seine_map_in_paris&gt;&gt;</span>
<span class="yarn-line">Paris grows around the Seine.</span> <span class="yarn-meta">#line:08cc02a </span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Draw a bridge in your notebook!</span> <span class="yarn-meta">#line:0b37e92 </span>
<span class="yarn-line">You can also find other activities on the website.</span> <span class="yarn-meta">#line:04f7aaa </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-protagonist"></a>

## protagonist

<div class="yarn-node" data-title="protagonist">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("find_photos")&gt;&gt;</span>
<span class="yarn-line">    Thank you!</span> <span class="yarn-meta">#line:0a70250</span>
<span class="yarn-line">    You found all my photos!</span> <span class="yarn-meta">#line:07cac48 </span>
<span class="yarn-line">    Now I can go home! One last question...</span> <span class="yarn-meta">#line:08b0aea </span>
    <span class="yarn-cmd">&lt;&lt;jump question_seine_map&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("collect_cards")&gt;&gt;</span>
<span class="yarn-line">    Thank you!</span> <span class="yarn-meta">#shadow:0a70250 </span>
<span class="yarn-line">    But I miss 7 photos about bridges and boats on the Seine!</span> <span class="yarn-meta">#line:006ab27 </span>
    <span class="yarn-cmd">&lt;&lt;area area_all&gt;&gt;</span>
<span class="yarn-line">    Explore the Seine and find my photos!</span> <span class="yarn-meta">#line:084c7e4 </span>
<span class="yarn-line">    If you can't find them, use the map!</span> <span class="yarn-meta">#line:0bfcb05 </span>
    <span class="yarn-cmd">&lt;&lt;task_start find_photos task_find_photos_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "find_photos" or GetCurrentTask() == "collect_cards"&gt;&gt;</span>
<span class="yarn-line">    You are doing well!</span> <span class="yarn-meta">#line:03ea48b </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Help! Help me!</span> <span class="yarn-meta">#line:0aa0f3a </span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_intro&gt;&gt;</span>
<span class="yarn-line">    I am a tourist and that big blue dog took my Paris guidebook!</span> <span class="yarn-meta">#line:0210ef0</span>
<span class="yarn-line">    And the pages with photos of the Seine are missing!</span> <span class="yarn-meta">#line:0c8e7b6</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive antura false&gt;&gt;</span>
<span class="yarn-line">    Find the photos around here!</span> <span class="yarn-meta">#line:0a9c8e7 #task:collect_cards</span>
    <span class="yarn-cmd">&lt;&lt;area area_tutorial&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start collect_cards task_collect_cards_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-collect-cards-done"></a>

## task_collect_cards_done

<div class="yarn-node" data-title="task_collect_cards_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You found all the photos!</span> <span class="yarn-meta">#line:found_photos</span>
<span class="yarn-line">Go back and talk to the tourist.</span> <span class="yarn-meta">#line:go_back_tourist #task:go_back</span>
<span class="yarn-cmd">&lt;&lt;target protagonist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_back&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-find-photos-done"></a>

## task_find_photos_done

<div class="yarn-node" data-title="task_find_photos_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You found all the photos!</span> <span class="yarn-meta">#shadow:found_photos</span>
<span class="yarn-line">Go back and talk to the tourist.</span> <span class="yarn-meta">#shadow:go_back_tourist #task:go_back</span>
<span class="yarn-cmd">&lt;&lt;target protagonist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_back&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-question-seine-map"></a>

## question_seine_map

<div class="yarn-node" data-title="question_seine_map">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The Seine flows into a sea. Which one?</span> <span class="yarn-meta">#line:066d7bd </span>
<span class="yarn-line">The Mediterranean Sea</span> <span class="yarn-meta">#line:02ab1e0 </span>
<span class="yarn-line">   Hmm... not really. Look at the map again.</span> <span class="yarn-meta">#line:099fe86 </span>
    <span class="yarn-cmd">&lt;&lt;jump question_seine_map&gt;&gt;</span>
<span class="yarn-line">The English Channel</span> <span class="yarn-meta">#line:03c92db </span>
    <span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-line">    Yes. The Seine flows into the English Channel, in the north of France.</span> <span class="yarn-meta">#line:0a2bcd3</span>
    <span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>
<span class="yarn-line">The Atlantic Ocean</span> <span class="yarn-meta">#line:052fdac </span>
<span class="yarn-line">    Hmm... not really. Look at the map again.</span> <span class="yarn-meta">#shadow:099fe86 </span>
    <span class="yarn-cmd">&lt;&lt;jump question_seine_map&gt;&gt;</span>
<span class="yarn-line">I don't know</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>

</code>
</pre>
</div>

<a id="ys-node-npc-boat-goods"></a>

## npc_boat_goods

<div class="yarn-node" data-title="npc_boat_goods">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "find_photos"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card boat_for_goods&gt;&gt;</span>
<span class="yarn-line">    This boat carries goods.</span> <span class="yarn-meta">#line:0f13a44</span>
    &lt;&lt;if $boat_goods &lt; 10&gt;&gt;
<span class="yarn-line">    What can you find on a goods boat?</span> <span class="yarn-meta">#line:04059bd </span>
<span class="yarn-line">    Boxes and things</span> <span class="yarn-meta">#line:0b12cb3</span>
        <span class="yarn-cmd">&lt;&lt;set $boat_goods = 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;target chest_boat_goods&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;camera_focus camera_chest_boat_goods&gt;&gt;</span>
<span class="yarn-line">        Yes! Now you can open that chest!</span> <span class="yarn-meta">#line:yes_chest</span>
        <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">    Tourists with cameras</span> <span class="yarn-meta">#line:0bbd3ce </span>
<span class="yarn-line">        No. Try again.</span> <span class="yarn-meta">#line:try_again </span>
        <span class="yarn-cmd">&lt;&lt;jump npc_boat_goods&gt;&gt;</span>
<span class="yarn-line">    Train cars</span> <span class="yarn-meta">#line:06638a3 </span>
<span class="yarn-line">        No. Try again.</span> <span class="yarn-meta">#shadow:try_again </span>
        <span class="yarn-cmd">&lt;&lt;jump npc_boat_goods&gt;&gt;</span>
<span class="yarn-line">    I don't know</span> <span class="yarn-meta">#line:dont_know #highlight </span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump spawned_river_friend&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Come back later.</span> <span class="yarn-meta">#line:come_back_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-boat-goods"></a>

## chest_boat_goods

<div class="yarn-node" data-title="chest_boat_goods">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $boat_goods == 1&gt;&gt;</span>
<span class="yarn-line">    Play a mini game to open the chest!</span> <span class="yarn-meta">#line:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_goods = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_boats chest_boat_goods&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_goods == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_boat_goods&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card boat_for_goods&gt;&gt;</span>
<span class="yarn-line">    The chest opens. You find a photo!</span> <span class="yarn-meta">#line:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_goods = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_goods == 10&gt;&gt;</span>
<span class="yarn-line">    The chest is empty.</span> <span class="yarn-meta">#line:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    The chest is locked.</span> <span class="yarn-meta">#line:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-boat-house"></a>

## npc_boat_house

<div class="yarn-node" data-title="npc_boat_house">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_house&gt;&gt;</span>
<span class="yarn-line">This boat is a house! People live here!</span> <span class="yarn-meta">#line:0a627c2 </span>
&lt;&lt;if $boat_house &lt; 10&gt;&gt;
<span class="yarn-line">Why is this a house boat?</span> <span class="yarn-meta">#line:0165157 </span>
<span class="yarn-line">People can live on it</span> <span class="yarn-meta">#line:0c86d26 </span>
    <span class="yarn-cmd">&lt;&lt;set $boat_house = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_boat_house&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_chest_boat_house&gt;&gt;</span>
<span class="yarn-line">    Yes! Now you can open that chest!</span> <span class="yarn-meta">#shadow:yes_chest</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">It is only for trains</span> <span class="yarn-meta">#line:055d822 </span>
<span class="yarn-line">    No. Try again.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_boat_house&gt;&gt;</span>
<span class="yarn-line">It can only carry goods</span> <span class="yarn-meta">#line:036ba15 </span>
<span class="yarn-line">    No. Try again.</span> <span class="yarn-meta">#shadow:try_again </span>
    <span class="yarn-cmd">&lt;&lt;jump npc_boat_house&gt;&gt;</span>
<span class="yarn-line">I don't know</span> <span class="yarn-meta">#shadow:dont_know #highlight </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_river_friend&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-boat-house"></a>

## chest_boat_house

<div class="yarn-node" data-title="chest_boat_house">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $boat_house == 1&gt;&gt;</span>
<span class="yarn-line">    Play a mini game to open the chest!</span> <span class="yarn-meta">#shadow:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_house = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_boat_house chest_boat_house&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_house == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_boat_house&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card boat_house&gt;&gt;</span>
<span class="yarn-line">    The chest opens. You find a photo!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_house = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_house == 10&gt;&gt;</span>
<span class="yarn-line">    The chest is empty.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    The chest is locked.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-boat-people"></a>

## npc_boat_people

<div class="yarn-node" data-title="npc_boat_people">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_people&gt;&gt;</span>
<span class="yarn-line">This boat has big windows, so tourists can see the city. It's a "bateau-mouche".</span> <span class="yarn-meta">#line:0129c99 </span>
&lt;&lt;if $boat_people &lt; 10&gt;&gt;
<span class="yarn-line">Why does a tourist boat have big windows?</span> <span class="yarn-meta">#line:02ce3de </span>
<span class="yarn-line">To see the city and famous places</span> <span class="yarn-meta">#line:0454688 </span>
    <span class="yarn-cmd">&lt;&lt;set $boat_people = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_boat_people&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_chest_boat_people&gt;&gt;</span>
<span class="yarn-line">    Yes! Now you can open that chest!</span> <span class="yarn-meta">#shadow:yes_chest</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">To carry more cars</span> <span class="yarn-meta">#line:02349a3</span>
<span class="yarn-line">    No. Try again.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_boat_people&gt;&gt;</span>
<span class="yarn-line">To go under the water</span> <span class="yarn-meta">#line:037efcf</span>
<span class="yarn-line">    No. Try again.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_boat_people&gt;&gt;</span>
<span class="yarn-line">I don't know</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_river_friend&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-boat-people"></a>

## chest_boat_people

<div class="yarn-node" data-title="chest_boat_people">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $boat_people == 1&gt;&gt;</span>
<span class="yarn-line">    Play a mini game to open the chest!</span> <span class="yarn-meta">#shadow:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_people = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity canvas_seine chest_boat_people&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_people == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_boat_people&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card boat_people&gt;&gt;</span>
<span class="yarn-line">    The chest opens. You find a photo!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_people = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_people == 10&gt;&gt;</span>
<span class="yarn-line">    The chest is empty.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    The chest is locked.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-bridge-alexandre"></a>

## npc_bridge_alexandre

<div class="yarn-node" data-title="npc_bridge_alexandre">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card pont_alexandre_iii&gt;&gt;</span>
<span class="yarn-line">This is the Pont Alexandre III, a famous old bridge.</span> <span class="yarn-meta">#line:09c706f</span>
&lt;&lt;if $bridge_alexandre &lt; 10&gt;&gt;
<span class="yarn-line">What is special about this famous bridge?</span> <span class="yarn-meta">#line:021f382</span>
<span class="yarn-line">It has gold statues</span> <span class="yarn-meta">#line:056c933</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_alexandre = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_bridge_alexandre&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_chest_bridge_alexandre&gt;&gt;</span>
<span class="yarn-line">    Yes! Now you can open that chest!</span> <span class="yarn-meta">#shadow:yes_chest</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">It is a train bridge</span> <span class="yarn-meta">#line:05c2518</span>
<span class="yarn-line">    No. Try again.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_bridge_alexandre&gt;&gt;</span>
<span class="yarn-line">It is made of ice</span> <span class="yarn-meta">#line:0d9bc29</span>
<span class="yarn-line">    No. Try again.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_bridge_alexandre&gt;&gt;</span>
<span class="yarn-line">I don't know</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_river_friend&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-bridge-alexandre"></a>

## chest_bridge_alexandre

<div class="yarn-node" data-title="chest_bridge_alexandre">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $bridge_alexandre == 1&gt;&gt;</span>
<span class="yarn-line">    Play a mini game to open the chest!</span> <span class="yarn-meta">#shadow:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_alexandre = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_pont chest_bridge_alexandre&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_alexandre == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_bridge_alexandre&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card pont_alexandre_iii&gt;&gt;</span>
<span class="yarn-line">    The chest opens. You find a photo!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_alexandre = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_alexandre == 10&gt;&gt;</span>
<span class="yarn-line">    The chest is empty.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    The chest is locked.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-bridge-cars"></a>

## npc_bridge_cars

<div class="yarn-node" data-title="npc_bridge_cars">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_cars&gt;&gt;</span>
<span class="yarn-line">This is a bridge for cars!</span> <span class="yarn-meta">#line:0d79d7d </span>
&lt;&lt;if $bridge_cars &lt; 10&gt;&gt;
<span class="yarn-line">Cars are fast. How do you cross safely?</span> <span class="yarn-meta">#line:01f30c4</span>
<span class="yarn-line">Use the sidewalk and cross at the crossing</span> <span class="yarn-meta">#line:0c8cdae </span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_cars = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_bridge_cars&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_chest_bridge_cars&gt;&gt;</span>
<span class="yarn-line">    Yes! Now you can open that chest!</span> <span class="yarn-meta">#shadow:yes_chest</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">Run across anywhere</span> <span class="yarn-meta">#line:076ee7f </span>
<span class="yarn-line">    No. Try again.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_bridge_cars&gt;&gt;</span>
<span class="yarn-line">Walk in the road</span> <span class="yarn-meta">#line:0cf371b</span>
<span class="yarn-line">    No. Try again.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_bridge_cars&gt;&gt;</span>
<span class="yarn-line">I don't know</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_river_friend&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-bridge-cars"></a>

## chest_bridge_cars

<div class="yarn-node" data-title="chest_bridge_cars">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $bridge_cars == 1&gt;&gt;</span>
<span class="yarn-line">    Play a mini game to open the chest!</span> <span class="yarn-meta">#shadow:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_cars = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity canvas_bridge_cars chest_bridge_cars&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_cars == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_bridge_cars&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card place_bridge_cars&gt;&gt;</span>
<span class="yarn-line">    The chest opens. You find a photo!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_cars = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_cars == 10&gt;&gt;</span>
<span class="yarn-line">    The chest is empty.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    The chest is locked.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-bridge-train"></a>

## npc_bridge_train

<div class="yarn-node" data-title="npc_bridge_train">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_trains&gt;&gt;</span>
<span class="yarn-line">This is a bridge for trains!</span> <span class="yarn-meta">#line:0a0991d </span>
&lt;&lt;if $bridge_train &lt; 10&gt;&gt;
<span class="yarn-line">A train bridge must be...</span> <span class="yarn-meta">#line:09731f9 </span>
<span class="yarn-line">Very strong</span> <span class="yarn-meta">#line:018a0eb </span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_bridge_train&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_chest_bridge_train&gt;&gt;</span>
<span class="yarn-line">    Yes! Now you can open that chest!</span> <span class="yarn-meta">#shadow:yes_chest</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">Soft and bouncy</span> <span class="yarn-meta">#line:08b4fad </span>
<span class="yarn-line">    No. Try again.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_bridge_train&gt;&gt;</span>
<span class="yarn-line">Only for walking</span> <span class="yarn-meta">#line:03ff912</span>
<span class="yarn-line">    No. Try again.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_bridge_train&gt;&gt;</span>
<span class="yarn-line">I don't know</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_river_friend&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-bridge-train"></a>

## chest_bridge_train

<div class="yarn-node" data-title="chest_bridge_train">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $bridge_train == 1&gt;&gt;</span>
<span class="yarn-line">    Play a mini game to open the chest!</span> <span class="yarn-meta">#shadow:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_bridges chest_bridge_train&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_train == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_bridge_train&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card place_bridge_trains&gt;&gt;</span>
<span class="yarn-line">    The chest opens. You find a photo!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_train == 10&gt;&gt;</span>
<span class="yarn-line">    The chest is empty.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    The chest is locked.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-train"></a>

## npc_train

<div class="yarn-node" data-title="npc_train">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I would love to ride a train!</span> <span class="yarn-meta">#line:01663fa </span>
<span class="yarn-line">I love trains!</span> <span class="yarn-meta">#line:0574872 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-bridge-people"></a>

## npc_bridge_people

<div class="yarn-node" data-title="npc_bridge_people">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card place_bridge_people&gt;&gt;</span>
<span class="yarn-line">This is a bridge for people!</span> <span class="yarn-meta">#line:0f8a96f </span>
&lt;&lt;if $bridge_people &lt; 10&gt;&gt;
<span class="yarn-line">A bridge only for people to walk on is called a...</span> <span class="yarn-meta">#line:0484ee9 </span>
<span class="yarn-line">Footbridge</span> <span class="yarn-meta">#line:0e48593 </span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_people = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_bridge_people&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_chest_bridge_people&gt;&gt;</span>
<span class="yarn-line">    Yes! Now you can open that chest!</span> <span class="yarn-meta">#shadow:yes_chest</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">Train bridge</span> <span class="yarn-meta">#line:0f74c53 </span>
<span class="yarn-line">    No. Try again.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_bridge_people&gt;&gt;</span>
<span class="yarn-line">Car bridge</span> <span class="yarn-meta">#line:07f732f </span>
<span class="yarn-line">    No. Try again.</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_bridge_people&gt;&gt;</span>
<span class="yarn-line">I don't know</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_river_friend&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-bridge-people"></a>

## chest_bridge_people

<div class="yarn-node" data-title="chest_bridge_people">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $bridge_people == 1&gt;&gt;</span>
<span class="yarn-line">    Play a mini game to open the chest!</span> <span class="yarn-meta">#shadow:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_people = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity match_bridges chest_bridge_people&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_people == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_bridge_people&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card place_bridge_people&gt;&gt;</span>
<span class="yarn-line">    The chest opens. You find a photo!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_people = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_people == 10&gt;&gt;</span>
<span class="yarn-line">    The chest is empty.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    The chest is locked.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-seine-map"></a>

## npc_seine_map

<div class="yarn-node" data-title="npc_seine_map">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card seine_map_in_paris&gt;&gt;</span>
<span class="yarn-line">Look at this map of the Seine.</span> <span class="yarn-meta">#line:08b1177 </span>
<span class="yarn-cmd">&lt;&lt;card seine_map&gt;&gt;</span>
<span class="yarn-line">The Seine goes north into the English Channel (a sea).</span> <span class="yarn-meta">#line:0281b33 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-boat"></a>

## npc_boat

<div class="yarn-node" data-title="npc_boat">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">No, I didn't see your dog.</span> <span class="yarn-meta">#line:0614aef </span>
<span class="yarn-line">A dog? There are many dogs here...</span> <span class="yarn-meta">#line:09f3f7a </span>
<span class="yarn-line">I did see a dog walking around, yes.</span> <span class="yarn-meta">#line:02ed36d </span>

</code>
</pre>
</div>

<a id="ys-node-npc-boat-eiffel"></a>

## npc_boat_eiffel

<div class="yarn-node" data-title="npc_boat_eiffel">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Look how nice!</span> <span class="yarn-meta">#line:059f66d </span>

</code>
</pre>
</div>

<a id="ys-node-npc-ile-de-france"></a>

## npc_ile_de_france

<div class="yarn-node" data-title="npc_ile_de_france">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card ile_de_france&gt;&gt;</span>
<span class="yarn-line">There is an island called Île de la Cité.</span> <span class="yarn-meta">#line:03e5667 </span>
<span class="yarn-line">It is in the old center of Paris.</span> <span class="yarn-meta">#line:04b2cf2 </span>

</code>
</pre>
</div>

<a id="ys-node-facts-river"></a>

## facts_river

<div class="yarn-node" data-title="facts_river">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The river starts far away and ends in the English Channel.</span> <span class="yarn-meta">#line:074efa5 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-french-woman"></a>

## spawned_french_woman

<div class="yarn-node" data-title="spawned_french_woman">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">spawn_group: residents </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour!</span> <span class="yarn-meta">#line:072a345 </span>
<span class="yarn-line">La Seine est magnifique!</span> <span class="yarn-meta">#line:0f0f558 </span>
<span class="yarn-line">Il y a beaucoup de ponts à Paris!</span> <span class="yarn-meta">#line:038b8f6 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-french-man"></a>

## spawned_french_man

<div class="yarn-node" data-title="spawned_french_man">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">spawn_group: residents </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Salut!</span> <span class="yarn-meta">#line:06aa0c1 </span>
<span class="yarn-line">J'aime bien faire du vélo le long de la Seine!</span> <span class="yarn-meta">#line:0494564 </span>
<span class="yarn-line">Paris est la plus belle ville du monde!</span> <span class="yarn-meta">#line:0d94ea8 </span>
<span class="yarn-line">La Seine est très longue!</span> <span class="yarn-meta">#line:0395113 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-bridge-expert"></a>

## spawned_bridge_expert

<div class="yarn-node" data-title="spawned_bridge_expert">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">spawn_group: bridge_expert</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">A train bridge must be strong.</span> <span class="yarn-meta">#line:0816b6b </span>
<span class="yarn-line">Some bridges are only for walking.</span> <span class="yarn-meta">#line:0ba2f29 </span>
<span class="yarn-line">Cars cross the river every day.</span> <span class="yarn-meta">#line:08d8ef0 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-river-friend"></a>

## spawned_river_friend

<div class="yarn-node" data-title="spawned_river_friend">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">spawn_group: river_friend</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The Seine helps boats move through Paris.</span> <span class="yarn-meta">#line:0ec822e </span>
<span class="yarn-line">Maps show how the river turns.</span> <span class="yarn-meta">#line:06d2470 </span>
<span class="yarn-line">The river water flows to the sea.</span> <span class="yarn-meta">#line:0d1724c </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-boat-guide"></a>

## spawned_boat_guide

<div class="yarn-node" data-title="spawned_boat_guide">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">spawn_group: boat_guide</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Tourist boats have big windows.</span> <span class="yarn-meta">#line:047502b </span>
<span class="yarn-line">Goods boats carry many boxes.</span> <span class="yarn-meta">#line:0ef47e5 </span>
<span class="yarn-line">A house boat can be a home.</span> <span class="yarn-meta">#line:00728bb </span>

</code>
</pre>
</div>


