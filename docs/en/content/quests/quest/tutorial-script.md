---
title: Tutorial (tutorial) - Script
hide:
---

# Tutorial (tutorial) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/quest-tutorial/41)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=631129787#gid=631129787)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/_TUTORIAL/Tutorial%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// tutorial | Tutorial</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Welcome to the Tutorial!</span> <span class="yarn-meta">#line:021793f </span>
<span class="yarn-cmd">&lt;&lt;if $IS_DESKTOP&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset keys_wasd&gt;&gt;</span>
<span class="yarn-line">    Use the keys WASD to walk.</span> <span class="yarn-meta">#line:037d71d </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset tutorial_move&gt;&gt;</span>
<span class="yarn-line">    Use your left finger to walk</span> <span class="yarn-meta">#line:0e55bc4 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_init&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target tutor_1&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-touchtext"></a>

## tutor_touchtext

<div class="yarn-node" data-title="tutor_touchtext">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Touch this text to hear it again.</span> <span class="yarn-meta">#line:03dbfa7 #native</span>
<span class="yarn-comment">// &lt;&lt;asset tutorial_goon&gt;&gt;</span>
<span class="yarn-comment">// Use this button to advance the dialog.</span> <span class="yarn-meta">#line:0f4f069 </span>

<span class="yarn-comment">//&lt;&lt;asset tutorial_image&gt;&gt;</span>
<span class="yarn-comment">//Use this button to view the photo</span> <span class="yarn-meta">#line:0784704 </span>

<span class="yarn-line">Go and talk to next tutor!</span> <span class="yarn-meta">#line:0eb85aa </span>
<span class="yarn-cmd">&lt;&lt;area area_medium&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-camera"></a>

## tutor_camera

<div class="yarn-node" data-title="tutor_camera">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $IS_DESKTOP&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset tutorial_camera_mouse&gt;&gt;</span>
<span class="yarn-line">    Press mouse button to move the camera.</span> <span class="yarn-meta">#line:0e633a2 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset tutorial_camera&gt;&gt;</span>
<span class="yarn-line">    Use your right finger to move the camera.</span> <span class="yarn-meta">#line:0aa47cb </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_large&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-act"></a>

## tutor_act

<div class="yarn-node" data-title="tutor_act">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_act&gt;&gt;</span>
<span class="yarn-line">Use this button to talk or interact</span> <span class="yarn-meta">#line:0c14f65 </span>
<span class="yarn-cmd">&lt;&lt;if $IS_DESKTOP&gt;&gt;</span>
<span class="yarn-line">    Or press SPACE key.</span> <span class="yarn-meta">#line:0c18f6b </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-jump"></a>

## tutor_jump

<div class="yarn-node" data-title="tutor_jump">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: asset=tutorial_move</span>
<span class="yarn-header-dim">---</span>
 <span class="yarn-cmd">&lt;&lt;asset tutorial_jump&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if $IS_DESKTOP&gt;&gt;</span>
<span class="yarn-line">    Press SPACE key or this button to jump</span> <span class="yarn-meta">#line:07940cf</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Use this button to jump</span> <span class="yarn-meta">#line:0b9c1fa </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_large&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-cookies"></a>

## tutor_cookies

<div class="yarn-node" data-title="tutor_cookies">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_cookies&gt;&gt;</span>
<span class="yarn-line">Take all cookies you find. They can be useful</span> <span class="yarn-meta">#line:0f50a6e </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-map"></a>

## tutor_map

<div class="yarn-node" data-title="tutor_map">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_map&gt;&gt;</span>
<span class="yarn-line">This button opens the map!</span> <span class="yarn-meta">#line:01777e4 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-interact"></a>

## tutor_interact

<div class="yarn-node" data-title="tutor_interact">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_actions&gt;&gt;</span>
<span class="yarn-line">Explore all objects that have this icon.</span> <span class="yarn-meta">#line:0139142 </span>
<span class="yarn-line">Find the key and open the door!</span> <span class="yarn-meta">#line:083e584 </span>

</code>
</pre>
</div>

<a id="ys-node-door-locked"></a>

## door_locked

<div class="yarn-node" data-title="door_locked">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    The key opens the door</span> <span class="yarn-meta">#line:0164ace </span>
    <span class="yarn-cmd">&lt;&lt;action deactivate_lock&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;trigger open_door&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    The door is locked</span> <span class="yarn-meta">#line:08766fc </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-pushball"></a>

## tutor_pushball

<div class="yarn-node" data-title="tutor_pushball">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_ball&gt;&gt;</span>
<span class="yarn-line">Try to push this ball.</span> <span class="yarn-meta">#line:02253ae </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-follow"></a>

## tutor_follow

<div class="yarn-node" data-title="tutor_follow">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_target&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target tutor_10&gt;&gt;</span>
<span class="yarn-line">This icon tells where to go.</span> <span class="yarn-meta">#line:06c117d </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-mission"></a>

## tutor_mission

<div class="yarn-node" data-title="tutor_mission">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_coin&gt;&gt;</span>
<span class="yarn-line">Let me see if you learned</span> <span class="yarn-meta">#line:0fe9efe</span>
<span class="yarn-cmd">&lt;&lt;target chest_end&gt;&gt;</span>
<span class="yarn-line">Climb the stairs and open that chest</span> <span class="yarn-meta">#line:07b2183 </span>
<span class="yarn-cmd">&lt;&lt;task_start open_chest task_open_chest_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-open-chest-done"></a>

## task_open_chest_done

<div class="yarn-node" data-title="task_open_chest_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great! You opened the chest!</span> <span class="yarn-meta">#line:0c30eb1 </span>
<span class="yarn-cmd">&lt;&lt;action appear_tutor_end&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;trigger tutor_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-end"></a>

## tutor_end

<div class="yarn-node" data-title="tutor_end">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: </span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;area area_all&gt;&gt;</span>
<span class="yarn-line">This is the end of the basic tutorial.</span> <span class="yarn-meta">#line:0dfdfc5 </span>
<span class="yarn-line">You can now explore the area and talk to everybody</span> <span class="yarn-meta">#line:02fea28 </span>
<span class="yarn-line">Are you ready to play?</span> <span class="yarn-meta">#line:0ac17d0 </span>
<span class="yarn-line">Yes</span> <span class="yarn-meta">#line:0b66e60 </span>
<span class="yarn-line">    Great. See you soon in the game!</span> <span class="yarn-meta">#line:07498c0 </span>
<span class="yarn-line">    There is much more to discover in Antura.</span> <span class="yarn-meta">#line:0ed06b6 </span>
<span class="yarn-line">No</span> <span class="yarn-meta">#line:01d5126 </span>
<span class="yarn-line">    You can replay the tutorial anytime you want.</span> <span class="yarn-meta">#line:06f9065 </span>
<span class="yarn-line">    Ask your teacher to help you.</span> <span class="yarn-meta">#line:0c6bc14 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-teleport"></a>

## tutor_teleport

<div class="yarn-node" data-title="tutor_teleport">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_portal&gt;&gt;</span>
<span class="yarn-line">Use the portals to travel fast!</span> <span class="yarn-meta">#line:0f753b5 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-killzone"></a>

## tutor_killzone

<div class="yarn-node" data-title="tutor_killzone">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_danger&gt;&gt;</span>
<span class="yarn-line">Pay attention to not fall into the water!</span> <span class="yarn-meta">#line:0e9b5a9 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-livingletter"></a>

## tutor_livingletter

<div class="yarn-node" data-title="tutor_livingletter">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_livingletter&gt;&gt;</span>
<span class="yarn-line">These are Living Letters. Talk to them to learn new words!</span> <span class="yarn-meta">#line:06e500f </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-blocky-character"></a>

## tutor_blocky_character

<div class="yarn-node" data-title="tutor_blocky_character">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_blocky_character&gt;&gt;</span>
<span class="yarn-line">These people are our friends. Talk to them to learn more about the world!</span> <span class="yarn-meta">#line:0be283b </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-card"></a>

## tutor_card

<div class="yarn-node" data-title="tutor_card">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_card&gt;&gt;</span>
<span class="yarn-line">This is a CARD. It has knowledge and powers. Collect them all!</span> <span class="yarn-meta">#line:0ac4c18 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-cat"></a>

## tutor_cat

<div class="yarn-node" data-title="tutor_cat">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_cat&gt;&gt;</span>
<span class="yarn-line">Yes. That is you! If you play all the game, you can change your look!</span> <span class="yarn-meta">#line:0eb1890 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-antura"></a>

## tutor_antura

<div class="yarn-node" data-title="tutor_antura">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura&gt;&gt;</span>
<span class="yarn-line">This is our friend Antura. He will help you in your adventure!</span> <span class="yarn-meta">#line:015cdc5 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-inventory"></a>

## tutor_inventory

<div class="yarn-node" data-title="tutor_inventory">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_inventory&gt;&gt;</span>
<span class="yarn-line">This is your inventory. Click on an object to use it.</span> <span class="yarn-meta">#line:02a6cfa </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-progress"></a>

## tutor_progress

<div class="yarn-node" data-title="tutor_progress">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_progress&gt;&gt;</span>
<span class="yarn-line">This is the game progress. Play well and get 3 stars!</span> <span class="yarn-meta">#line:0b606b8 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-tasks"></a>

## tutor_tasks

<div class="yarn-node" data-title="tutor_tasks">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_tasks&gt;&gt;</span>
<span class="yarn-line">This panel tells you what you have to do.</span> <span class="yarn-meta">#line:012d967 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-target"></a>

## tutor_target

<div class="yarn-node" data-title="tutor_target">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_target&gt;&gt;</span>
<span class="yarn-line">This symbol shows you where to go.</span> <span class="yarn-meta">#line:0864faf </span>

</code>
</pre>
</div>

<a id="ys-node-activity-memory"></a>

## activity_memory

<div class="yarn-node" data-title="activity_memory">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Let's play Activity MEMORY!</span> <span class="yarn-meta">#line:05e100e </span>
<span class="yarn-line">Find the pairs of cards.</span> <span class="yarn-meta">#line:0ef88ae </span>
<span class="yarn-cmd">&lt;&lt;activity memory_tutorial activity_memory_result tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-memory-result"></a>

## activity_memory_result

<div class="yarn-node" data-title="activity_memory_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">It's good to exercise our memory!</span> <span class="yarn-meta">#line:00e6a04 </span>
<span class="yarn-line">Do you want to play again?</span> <span class="yarn-meta">#line:0c78a9e </span>
<span class="yarn-line">Easy</span> <span class="yarn-meta">#line:0cd0316 </span>
    <span class="yarn-cmd">&lt;&lt;activity memory_tutorial activity_memory_result easy&gt;&gt;</span>
<span class="yarn-line">Normal</span> <span class="yarn-meta">#line:07cbfdd </span>
    <span class="yarn-cmd">&lt;&lt;activity memory_tutorial activity_memory_result normal&gt;&gt;</span>
<span class="yarn-line">Expert</span> <span class="yarn-meta">#line:0e3251c </span>
    <span class="yarn-cmd">&lt;&lt;activity memory_tutorial activity_memory_result expert&gt;&gt;</span>
<span class="yarn-line">No</span> <span class="yarn-meta">#line:010515b </span>

</code>
</pre>
</div>

<a id="ys-node-activity-canvas"></a>

## activity_canvas

<div class="yarn-node" data-title="activity_canvas">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Let's play Activity CANVAS</span> <span class="yarn-meta">#line:07041b3 </span>
<span class="yarn-line">You have to clean all the screen without touching Antura!</span> <span class="yarn-meta">#line:0c80adc </span>
<span class="yarn-cmd">&lt;&lt;activity canvas_tutorial activity_canvas_result tutorial&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-activity-canvas-result"></a>

## activity_canvas_result

<div class="yarn-node" data-title="activity_canvas_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">It's nice to clean, isn't it?</span> <span class="yarn-meta">#line:059323a</span>
Do you want to play again? #shadow:0c78a9e
-&gt; Easy #shadow:0cd0316
    <span class="yarn-cmd">&lt;&lt;activity canvas_tutorial activity_canvas_result easy&gt;&gt;</span>
-&gt; Normal #shadow:07cbfdd
    <span class="yarn-cmd">&lt;&lt;activity canvas_tutorial activity_canvas_result normal&gt;&gt;</span>
-&gt; Expert #shadow:0e3251c
    <span class="yarn-cmd">&lt;&lt;activity canvas_tutorial activity_canvas_result expert&gt;&gt;</span>
-&gt; No #highlight #shadow:010515b

</code>
</pre>
</div>

<a id="ys-node-activity-jigsaw"></a>

## activity_jigsaw

<div class="yarn-node" data-title="activity_jigsaw">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Let's play Activity JIGSAW PUZZLE!</span> <span class="yarn-meta">#line:0fe648a </span>
<span class="yarn-line">Complete the image.</span> <span class="yarn-meta">#line:0bc50ca </span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_tutorial activity_jigsaw_result tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-jigsaw-result"></a>

## activity_jigsaw_result

<div class="yarn-node" data-title="activity_jigsaw_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I love puzzles!</span> <span class="yarn-meta">#line:0fc42d8 </span>
Do you want to play again? #shadow:0c78a9e
-&gt; Easy #shadow:0cd0316
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_tutorial activity_jigsaw_result easy&gt;&gt;</span>
-&gt; Normal #shadow:07cbfdd
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_tutorial activity_jigsaw_result normal&gt;&gt;</span>
-&gt; Expert #shadow:0e3251c
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_tutorial activity_jigsaw_result expert&gt;&gt;</span>
-&gt; No #highlight #shadow:010515b

</code>
</pre>
</div>

<a id="ys-node-activity-match"></a>

## activity_match

<div class="yarn-node" data-title="activity_match">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Let's play Activity MATCH!</span> <span class="yarn-meta">#line:07c8447 </span>
<span class="yarn-line">Match the similar cards.</span> <span class="yarn-meta">#line:02aec90 </span>
<span class="yarn-cmd">&lt;&lt;activity match_tutorial activity_match_result tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match-result"></a>

## activity_match_result

<div class="yarn-node" data-title="activity_match_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Nice matching!</span> <span class="yarn-meta">#line:0f0ee2e</span>
Do you want to play again? #shadow:0c78a9e
-&gt; Easy #shadow:0cd0316
    <span class="yarn-cmd">&lt;&lt;activity match_tutorial activity_match_result easy&gt;&gt;</span>
-&gt; Normal #shadow:07cbfdd
    <span class="yarn-cmd">&lt;&lt;activity match_tutorial activity_match_result normal&gt;&gt;</span>
-&gt; Expert #shadow:0e3251c
    <span class="yarn-cmd">&lt;&lt;activity match_tutorial activity_match_result expert&gt;&gt;</span>
-&gt; No #highlight #shadow:010515b

</code>
</pre>
</div>

<a id="ys-node-activity-money"></a>

## activity_money

<div class="yarn-node" data-title="activity_money">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Let's play Activity COUNT THE MONEY!</span> <span class="yarn-meta">#line:06b295f </span>
<span class="yarn-line">You have to give the correct amount of coins.</span> <span class="yarn-meta">#line:0f00e5d </span>
<span class="yarn-cmd">&lt;&lt;activity money_tutorial activity_money_result tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-money-result"></a>

## activity_money_result

<div class="yarn-node" data-title="activity_money_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">It's important to learn how to use money!</span> <span class="yarn-meta">#line:06136c1</span>
Do you want to play again? #shadow:0c78a9e
-&gt; Easy #shadow:0cd0316
    <span class="yarn-cmd">&lt;&lt;activity money_tutorial activity_money_result easy&gt;&gt;</span>
-&gt; Normal #shadow:07cbfdd
    <span class="yarn-cmd">&lt;&lt;activity money_tutorial activity_money_result normal&gt;&gt;</span>
-&gt; Expert #shadow:0e3251c
    <span class="yarn-cmd">&lt;&lt;activity money_tutorial activity_money_result expert&gt;&gt;</span>
-&gt; No #highlight #shadow:010515b

</code>
</pre>
</div>

<a id="ys-node-activity-order"></a>

## activity_order

<div class="yarn-node" data-title="activity_order">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Let's play Activity ORDER!</span> <span class="yarn-meta">#line:015a3ea </span>
<span class="yarn-line">Put the items in the correct order.</span> <span class="yarn-meta">#line:0ed152d </span>
<span class="yarn-cmd">&lt;&lt;activity order_tutorial activity_order_result&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-order-result"></a>

## activity_order_result

<div class="yarn-node" data-title="activity_order_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Good activity.</span> <span class="yarn-meta">#line:0838f7f </span>
Do you want to play again? #shadow:0c78a9e
-&gt; Easy #shadow:0cd0316
    <span class="yarn-cmd">&lt;&lt;activity order_tutorial activity_order_result easy&gt;&gt;</span>
-&gt; Normal #shadow:07cbfdd
    <span class="yarn-cmd">&lt;&lt;activity order_tutorial activity_order_result normal&gt;&gt;</span>
-&gt; Expert #shadow:0e3251c
    <span class="yarn-cmd">&lt;&lt;activity order_tutorial activity_order_result expert&gt;&gt;</span>
-&gt; No #highlight #shadow:010515b

</code>
</pre>
</div>

<a id="ys-node-ll-skyscraper"></a>

## ll_skyscraper

<div class="yarn-node" data-title="ll_skyscraper">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">It's wonderful to see the city from up here!</span> <span class="yarn-meta">#line:0969b2f </span>
<span class="yarn-line">I would like to travel all around the world!</span> <span class="yarn-meta">#line:060db75 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-secrets"></a>

## npc_secrets

<div class="yarn-node" data-title="npc_secrets">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I heard there are secrets in this level...</span> <span class="yarn-meta">#line:0b8d755 </span>
<span class="yarn-line">And a portal to travel on that skyscraper!</span> <span class="yarn-meta">#line:032af93 </span>
<span class="yarn-cmd">&lt;&lt;jump global_init&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-hallo"></a>

## npc_hallo

<div class="yarn-node" data-title="npc_hallo">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hallo!</span> <span class="yarn-meta">#line:022bd3f</span>
<span class="yarn-line">have you seen Antura?</span> <span class="yarn-meta">#line:00ad419</span>
<span class="yarn-line">I lost my cookie!</span> <span class="yarn-meta">#line:0a94666 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-kid"></a>

## npc_kid

<div class="yarn-node" data-title="npc_kid">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cookies are hidden all around! Find them!</span> <span class="yarn-meta">#line:0c8d2fa </span>
<span class="yarn-line">I love to use the portals to travel!</span> <span class="yarn-meta">#line:0fb6855 </span>
<span class="yarn-line">Talk to everyone you meet!</span> <span class="yarn-meta">#line:0812c45 </span>
<span class="yarn-line">Explore the world!</span> <span class="yarn-meta">#line:041d845 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-arcade"></a>

## npc_arcade

<div class="yarn-node" data-title="npc_arcade">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I love videogames!</span> <span class="yarn-meta">#line:0479595 </span>
<span class="yarn-line">Do you want to play with me?</span> <span class="yarn-meta">#line:0d3fdc7 </span>

</code>
</pre>
</div>


