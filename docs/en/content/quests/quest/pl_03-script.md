---
title: A Voyage on the Odra River (pl_03) - Script
hide:
---

# A Voyage on the Odra River (pl_03) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-03-a-voyage-on-the-odra-river/34/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=106202032#gid=106202032)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_03%20Wroclaw%20River/PL_03%20Wroclaw%20River%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_03 | Odra River (Wroclaw)</span>
<span class="yarn-header-dim">// PL_03 - A Voyage on the Odra River</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 8&gt;&gt;</span>
<span class="yarn-comment">// State variables for the 8 chests</span>
<span class="yarn-cmd">&lt;&lt;declare $map_odra = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $river_sign = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $bridge_tumski = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $bridge_redzinski = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $bridge_train = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $boat_house = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $boat_tourist = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $boat_barge = 0&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;card place_odra_river&gt;&gt;</span>
<span class="yarn-line">We are in Wrocław, the "City of a Hundred Bridges".</span> <span class="yarn-meta">#line:start_1</span>
<span class="yarn-line">Today we will explore the river, bridges and boats.</span> <span class="yarn-meta">#line:start_2</span>
<span class="yarn-cmd">&lt;&lt;target protagonist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_init&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Well done!</span> <span class="yarn-meta">#line:02a257c </span>
<span class="yarn-cmd">&lt;&lt;card bridge&gt;&gt;</span>
<span class="yarn-line">We learned about different types of bridges.</span> <span class="yarn-meta">#line:end_1</span>
<span class="yarn-cmd">&lt;&lt;card boat&gt;&gt;</span>
<span class="yarn-line">We learned about different types of boats.</span> <span class="yarn-meta">#line:end_2</span>
<span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span>
<span class="yarn-line">We explored the ODRA RIVER.</span> <span class="yarn-meta">#line:end_3</span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Draw a simple MAP of your journey.</span> <span class="yarn-meta">#line:0265d07 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-protagonist"></a>

## protagonist

<div class="yarn-node" data-title="protagonist">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("find_photos")&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! You found all my photos!</span> <span class="yarn-meta">#line:prot_1</span>
<span class="yarn-line">    Now I can continue the visit. One last question...</span> <span class="yarn-meta">#line:prot_2</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("collect_cards")&gt;&gt;</span>
<span class="yarn-line">    You are brave! But I still miss photos of our river wonders.</span> <span class="yarn-meta">#line:006ab27 </span>
    <span class="yarn-cmd">&lt;&lt;area area_all&gt;&gt;</span>
<span class="yarn-line">    Look for the chests! The people by the river watch over them.</span> <span class="yarn-meta">#line:prot_4 </span>
<span class="yarn-line">    If you can't find them, use the map!</span> <span class="yarn-meta">#line:0bfcb05 </span>
    <span class="yarn-cmd">&lt;&lt;task_start find_photos task_find_photos_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "find_photos" or GetCurrentTask() == "collect_cards"&gt;&gt;</span>
<span class="yarn-line">    You are doing well!</span> <span class="yarn-meta">#line:03ea48b </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Help! Antura thought my Wrocław Guide was a bone!</span> <span class="yarn-meta">#line:prot_5</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_intro&gt;&gt;</span>
<span class="yarn-line">    The pages are lost around. Can you find them?</span> <span class="yarn-meta">#line:prot_6 #task:collect_cards</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive antura false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;area area_tutorial&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start collect_cards task_collect_cards_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-find-photos-done"></a>

## task_find_photos_done

<div class="yarn-node" data-title="task_find_photos_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You found all the photos!</span> <span class="yarn-meta">#line:found_photos</span>
<span class="yarn-line">Go back and talk to the guide.</span> <span class="yarn-meta">#line:go_back #task:go_back </span>
<span class="yarn-cmd">&lt;&lt;target protagonist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_back&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-collect-cards-done"></a>

## task_collect_cards_done

<div class="yarn-node" data-title="task_collect_cards_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You found all the photos!</span> <span class="yarn-meta">#shadow:found_photos</span>
<span class="yarn-line">Go back and talk to the guide.</span> <span class="yarn-meta">#shadow:go_back #task:go_back</span>
<span class="yarn-cmd">&lt;&lt;target protagonist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_back&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-final-quiz"></a>

## final_quiz

<div class="yarn-node" data-title="final_quiz">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The Odra is a long river. Do you remember where all that water ends up?</span> <span class="yarn-meta">#line:quiz1_intro</span>
<span class="yarn-line">The Baltic Sea</span> <span class="yarn-meta">#line:quiz_a1</span>
    <span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span>
<span class="yarn-line">    Tak! Correct! The Odra flows north across Poland and into the Baltic Sea.</span> <span class="yarn-meta">#line:quiz1_ok</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz_2&gt;&gt;</span>
<span class="yarn-line">The Mediterranean Sea</span> <span class="yarn-meta">#line:quiz_a2</span>
<span class="yarn-line">    Hmm... that's much too far south!</span> <span class="yarn-meta">#line:quiz1_fail1</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz&gt;&gt;</span>
<span class="yarn-line">The Black Sea</span> <span class="yarn-meta">#line:quiz_a3</span>
<span class="yarn-line">    Not quite. The Odra heads North, not South!</span> <span class="yarn-meta">#line:quiz1_fail2</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz&gt;&gt;</span>
<span class="yarn-line">I don't know</span> <span class="yarn-meta">#line:dontknow</span>
    <span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span> 
<span class="yarn-line">    No worries! Look at the blue line on the map.</span> <span class="yarn-meta">#line:quiz1_hint</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-final-quiz-2"></a>

## final_quiz_2

<div class="yarn-node" data-title="final_quiz_2">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Is Odra the *longest* river in Poland?</span> <span class="yarn-meta">#line:quiz2_intro</span>
<span class="yarn-line">No, the Wisła is longer</span> <span class="yarn-meta">#line:quiz2_a1</span>
    <span class="yarn-cmd">&lt;&lt;card place_vistula_river&gt;&gt;</span>
<span class="yarn-line">    Perfect! The Wisła is number one, Odra is the second longest.</span> <span class="yarn-meta">#line:quiz2_ok</span>
    <span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>
<span class="yarn-line">Yes, it is the longest</span> <span class="yarn-meta">#line:quiz2_a2</span>
<span class="yarn-line">    It is very big, but there is one river that is even longer.</span> <span class="yarn-meta">#line:quiz2_fail</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz_2&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-river-sign"></a>

## npc_river_sign

<div class="yarn-node" data-title="npc_river_sign">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">// ---------- RIVER SIGN</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $river_sign &lt; 10&gt;&gt;
<span class="yarn-cmd">&lt;&lt;card river_sign&gt;&gt;</span>
<span class="yarn-line">Look at the big blue sign near the bridge.</span> <span class="yarn-meta">#line:sign_1</span>
<span class="yarn-line">What do the white wavy lines tell us?</span> <span class="yarn-meta">#line:sign_3</span>
<span class="yarn-line">There is a river flowing here</span> <span class="yarn-meta">#line:sign_4</span>
    <span class="yarn-cmd">&lt;&lt;set $river_sign = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_river_sign&gt;&gt;</span>
<span class="yarn-line">    Tak! Those blue waves are the universal sign for a river.</span> <span class="yarn-meta">#line:sign_5</span>
<span class="yarn-line">There bridge moves</span> <span class="yarn-meta">#line:sign_6</span>
<span class="yarn-line">    No. Try again.</span> <span class="yarn-meta">#line:try_again </span>
<span class="yarn-line">I don't know</span> <span class="yarn-meta">#line:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-river-sign"></a>

## chest_river_sign

<div class="yarn-node" data-title="chest_river_sign">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $river_sign == 1&gt;&gt;</span>
<span class="yarn-line">    Antura covered the sign in mud! Wipe it clean to see the waves.</span> <span class="yarn-meta">#line:ch_sign1</span>
    <span class="yarn-cmd">&lt;&lt;set $river_sign = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity clean_river_sign chest_river_sign&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $river_sign == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_sign&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card river_sign&gt;&gt;</span>
<span class="yarn-line">    Great job! Now you'll always know when you're crossing a river in Europe.</span> <span class="yarn-meta">#line:ch_sign2</span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $river_sign = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $river_sign == 10&gt;&gt;</span>
<span class="yarn-line">    The chest is empty.</span> <span class="yarn-meta">#line:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    The chest is locked.</span> <span class="yarn-meta">#line:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-odra-map"></a>

## npc_odra_map

<div class="yarn-node" data-title="npc_odra_map">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">// ---------- ODRA RIVER MAP</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span>
<span class="yarn-line">The Odra is Poland's second longest river.</span> <span class="yarn-meta">#line:map_1</span>
&lt;&lt;if $map_odra &lt; 10&gt;&gt;
<span class="yarn-line">Does the Odra flow into the mountains or the sea?</span> <span class="yarn-meta">#line:map_2</span>
<span class="yarn-line">The Baltic Sea</span> <span class="yarn-meta">#line:map_3</span>
    <span class="yarn-cmd">&lt;&lt;set $map_odra = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_odra_map&gt;&gt;</span>
<span class="yarn-line">    Correct! It flows all the way to the north.</span> <span class="yarn-meta">#line:map_4</span>
<span class="yarn-line">The Mediterranean Sea</span> <span class="yarn-meta">#line:map_5</span>
<span class="yarn-line">    No. Try again.</span> <span class="yarn-meta">#shadow:try_again </span>
    <span class="yarn-cmd">&lt;&lt;jump npc_odra_map&gt;&gt;</span>
<span class="yarn-line">I don't know</span> <span class="yarn-meta">#shadow:dont_know #highlight </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-odra-map"></a>

## chest_odra_map

<div class="yarn-node" data-title="chest_odra_map">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $map_odra == 1&gt;&gt;</span>
<span class="yarn-line">    Prove you know where the river flows!</span> <span class="yarn-meta">#line:ch_map1</span>
    <span class="yarn-cmd">&lt;&lt;set $map_odra = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_odra_facts chest_odra_river_map&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $map_odra == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_map&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span>
<span class="yarn-line">    The map is back! It shows the Odra flowing into the Baltic Sea.</span> <span class="yarn-meta">#line:ch_map2</span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $map_odra = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_barge == 10&gt;&gt;</span>
<span class="yarn-line">    The chest is empty.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    The chest is locked.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-tumski-bridge"></a>

## npc_tumski_bridge

<div class="yarn-node" data-title="npc_tumski_bridge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- TUMSKI BRIDGE (The Romantic Bridge)</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card tumski_bridge&gt;&gt;</span>
<span class="yarn-line">This is the Tumski Bridge. It goes to the oldest part of the city.</span> <span class="yarn-meta">#line:tum_1</span>
<span class="yarn-line">Every evening, a man lights the 102 gas lanterns here by hand!</span> <span class="yarn-meta">#line:tum_2</span>
&lt;&lt;if $bridge_tumski &lt; 10&gt;&gt;
<span class="yarn-line">What do couples hang on this bridge for luck and love?</span> <span class="yarn-meta">#line:tum_3</span>
<span class="yarn-line">Padlocks</span> <span class="yarn-meta">#line:tum_4</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_tumski = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_tumski_bridge&gt;&gt;</span>
<span class="yarn-line">    Yes! Though they are very heavy and will be removed!</span> <span class="yarn-meta">#line:tum_5</span>
<span class="yarn-line">Wet socks</span> <span class="yarn-meta">#line:tum_6</span>
<span class="yarn-line">    That would not be very romantic! Try again.</span> <span class="yarn-meta">#line:fail_tum</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_tumski_bridge&gt;&gt;</span>
<span class="yarn-line">I don't know</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-tumski-bridge"></a>

## chest_tumski_bridge

<div class="yarn-node" data-title="chest_tumski_bridge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $bridge_tumski == 1&gt;&gt;</span>
<span class="yarn-line">    Clean the rust off this old iron bridge!</span> <span class="yarn-meta">#line:ch_tum1</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_tumski = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity cleancanvas odra_footbridge chest_tumski_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_tumski == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_tumski&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card tumski_bridge&gt;&gt;</span>
<span class="yarn-line">    The chest opens. You find a photo!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_tumski = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_tumski == 10&gt;&gt;</span>
<span class="yarn-line">    The chest is empty.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    The chest is locked.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-redzinski-bridge"></a>

## npc_redzinski_bridge

<div class="yarn-node" data-title="npc_redzinski_bridge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- RĘDZIŃSKI BRIDGE</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">The Rędziński Bridge is the tallest and longest in Poland.</span> <span class="yarn-meta">#line:redz_1</span>
<span class="yarn-line">It is 122 meters tall. Higher than the Cathedral!</span> <span class="yarn-meta">#line:redz_2</span>
&lt;&lt;if $bridge_redzinski &lt; 10&gt;&gt;
<span class="yarn-line">What holds this massive bridge up?</span> <span class="yarn-meta">#line:redz_3</span>
<span class="yarn-line">Steel cables</span> <span class="yarn-meta">#line:redz_4</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_redzinski = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">    Correct! Strong ropes hold this giant bridge. Cars use it to drive around the city.</span> <span class="yarn-meta">#line:redz_5</span>
<span class="yarn-line">Magnets and Magic</span> <span class="yarn-meta">#line:redz_6</span>
<span class="yarn-line">    It looks like magic, but it's actually engineering!</span> <span class="yarn-meta">#line:fail_redz</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">I don't know</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-redzinski-bridge"></a>

## chest_redzinski_bridge

<div class="yarn-node" data-title="chest_redzinski_bridge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $bridge_redzinski == 1&gt;&gt;</span>
<span class="yarn-line">    Rebuild the tallest pylon in Wrocław!</span> <span class="yarn-meta">#line:ch_redz1</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_redzinski = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_pont chest_redzinski_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_redzinski == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_redzinski&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">    The chest opens. You find a photo!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_redzinski = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_redzinski == 10&gt;&gt;</span>
<span class="yarn-line">    The chest is empty.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    The chest is locked.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-train-bridge"></a>

## npc_train_bridge

<div class="yarn-node" data-title="npc_train_bridge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- TRAIN BRIDGE</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card train_bridge&gt;&gt;</span>
<span class="yarn-line">Trains have been crossing the Odra in Wrocław for over 150 years.</span> <span class="yarn-meta">#line:train_1</span>
&lt;&lt;if $bridge_train &lt; 10&gt;&gt;
<span class="yarn-line">Why are train bridges made of such heavy steel?</span> <span class="yarn-meta">#line:train_2</span>
<span class="yarn-line">Because trains are very heavy</span> <span class="yarn-meta">#line:train_3</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_bridge_train&gt;&gt;</span>
<span class="yarn-line">    Yes! It must be strong enough for the heavy trains.</span> <span class="yarn-meta">#line:train_4</span>
<span class="yarn-line">To make a loud noise</span> <span class="yarn-meta">#line:train_5</span>
<span class="yarn-line">    They are loud, but that's not why!</span> <span class="yarn-meta">#line:fail_train</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_train_bridge&gt;&gt;</span>
<span class="yarn-line">I don't know</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-bridge-train"></a>

## chest_bridge_train

<div class="yarn-node" data-title="chest_bridge_train">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $bridge_train == 1&gt;&gt;</span>
<span class="yarn-line">    Match the heavy boxes to the train tracks!</span> <span class="yarn-meta">#line:ch_train1</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_bridges chest_bridge_train&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_train == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_train&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card train_bridge&gt;&gt;</span>
<span class="yarn-line">    The chest opens. You find a photo!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_train == 10&gt;&gt;</span>
<span class="yarn-line">    The chest is empty.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    The chest is locked.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-line">I don't know</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-houseboat"></a>

## npc_houseboat

<div class="yarn-node" data-title="npc_houseboat">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- HOUSEBOAT</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card houseboat&gt;&gt;</span>
<span class="yarn-line">In Wrocław, some people call the river their "home street."</span> <span class="yarn-meta">#line:house_1</span>
<span class="yarn-line">Even the dwarves would love a floating house!</span> <span class="yarn-meta">#line:house_2</span>
&lt;&lt;if $boat_house &lt; 10&gt;&gt;
<span class="yarn-line">If you live on a houseboat, what do you use for a backyard?</span> <span class="yarn-meta">#line:house_3</span>
<span class="yarn-line">The River Odra</span> <span class="yarn-meta">#line:house_4</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_house = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_houseboat&gt;&gt;</span>
<span class="yarn-line">    The chest opens. You find a photo!</span> <span class="yarn-meta">#shadow:chest_opens </span>
<span class="yarn-line">A rooftop forest</span> <span class="yarn-meta">#line:house_6</span>
<span class="yarn-line">    No. Try again.</span> <span class="yarn-meta">#shadow:try_again </span>
    <span class="yarn-cmd">&lt;&lt;jump npc_houseboat&gt;&gt;</span>
<span class="yarn-line">I don't know</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-houseboat"></a>

## chest_houseboat

<div class="yarn-node" data-title="chest_houseboat">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $boat_house == 1&gt;&gt;</span>
<span class="yarn-line">    Fix the windows on the floating house!</span> <span class="yarn-meta">#line:ch_house1</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_house = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_boat_house chest_houseboat&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_house == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_houseboat&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card houseboat&gt;&gt;</span>
<span class="yarn-line">    A cozy home on the Odra! Photo collected.</span> <span class="yarn-meta">#line:ch_house2</span>
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
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- BOAT PEOPLE (Tourist Boats)</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_people&gt;&gt;</span>
<span class="yarn-line">Tourist boats take people to see the Zoo and the Cathedral.</span> <span class="yarn-meta">#line:tour_1</span>
&lt;&lt;if $boat_tourist &lt; 10&gt;&gt;
<span class="yarn-line">What do the people on these boats use to see the sights?</span> <span class="yarn-meta">#line:tour_2</span>
<span class="yarn-line">Their eyes and cameras</span> <span class="yarn-meta">#line:tour_3</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_tourist = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_boat_people&gt;&gt;</span>
<span class="yarn-line">    Yes! Smile for the photo!</span> <span class="yarn-meta">#line:tour_4</span>
<span class="yarn-line">A periscope</span> <span class="yarn-meta">#line:tour_5</span>
<span class="yarn-line">    We aren't underwater yet! Try again.</span> <span class="yarn-meta">#line:fail_tour</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_boat_people&gt;&gt;</span>
<span class="yarn-line">I don't know</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-boat-people"></a>

## chest_boat_people

<div class="yarn-node" data-title="chest_boat_people">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $boat_tourist == 1&gt;&gt;</span>
<span class="yarn-line">    Find the tourists hidden on the deck!</span> <span class="yarn-meta">#line:ch_tour1</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_tourist = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_boats chest_boat_people&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_tourist == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_tourist&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card boat_people&gt;&gt;</span>
<span class="yarn-line">    The chest opens. You find a photo!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_tourist = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_tourist == 10&gt;&gt;</span>
<span class="yarn-line">    The chest is empty.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    The chest is locked.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-boat-barge"></a>

## npc_boat_barge

<div class="yarn-node" data-title="npc_boat_barge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- BARGE (Cargo)</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card barge&gt;&gt;</span>
<span class="yarn-line">Barges (Barki) have carried coal and sand on the Odra for centuries.</span> <span class="yarn-meta">#line:barge_1</span>
&lt;&lt;if $boat_barge &lt; 10&gt;&gt;
<span class="yarn-line">A barge is very flat. Why?</span> <span class="yarn-meta">#line:barge_2</span>
<span class="yarn-line">To carry heavy things even when the water is not deep.</span> <span class="yarn-meta">#line:barge_3</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_barge = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_barge&gt;&gt;</span>
<span class="yarn-line">    Exactly! It's a truck that floats.</span> <span class="yarn-meta">#line:barge_4</span>
<span class="yarn-line">So it can hide from dwarves</span> <span class="yarn-meta">#line:barge_5</span>
<span class="yarn-line">    No. Try again.</span> <span class="yarn-meta">#shadow:try_again </span>
    <span class="yarn-cmd">&lt;&lt;jump npc_boat_barge&gt;&gt;</span>
<span class="yarn-line">I don't know</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-boat-barge"></a>

## chest_boat_barge

<div class="yarn-node" data-title="chest_boat_barge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $boat_barge == 1&gt;&gt;</span>
<span class="yarn-line">    Play a mini game to open the chest!</span> <span class="yarn-meta">#line:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_barge = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_boats chest_boat_barge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_barge == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_boat_barge&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card barge&gt;&gt;</span>
<span class="yarn-line">    The chest opens. You find a photo!</span> <span class="yarn-meta">#line:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_barge = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_barge == 10&gt;&gt;</span>
<span class="yarn-line">    The chest is empty.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    The chest is locked.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kayak"></a>

## spawned_kayak

<div class="yarn-node" data-title="spawned_kayak">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawned automatically in the scene</span>
<span class="yarn-header-dim">// each time you meet them they say one random line</span>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I want to ride a small KAYAK.</span> <span class="yarn-meta">#line:0f36b7f #card:kayak</span>
<span class="yarn-line">Kayaks are great for exploring nature!</span> <span class="yarn-meta">#line:kayak_2 #card:kayak</span>
<span class="yarn-line">Paddling is fun and good exercise!</span> <span class="yarn-meta">#line:kayak_3</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-tourist"></a>

## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">So many BRIDGES in this city.</span> <span class="yarn-meta">#line:0577d80 </span>
<span class="yarn-line">Wroclaw is really beautiful.</span> <span class="yarn-meta">#line:089ea37 </span>
<span class="yarn-line">I love Pierogi!</span> <span class="yarn-meta">#line:07ff8c5 </span>
<span class="yarn-line">The Cathedral island is magical at night.</span> <span class="yarn-meta">#line:tourist_4</span>

</code>
</pre>
</div>


