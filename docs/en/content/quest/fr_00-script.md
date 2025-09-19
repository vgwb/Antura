---
title: The neighbors of France (fr_00) - Script
hide:
---

# The neighbors of France (fr_00) - Script
[Quest Index](./index.md) - Language: english - [french](./fr_00-script.fr.md) - [polish](./fr_00-script.pl.md) - [italian](./fr_00-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-00-the-neighbors-of-france/22)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1044148815#gid=1044148815)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_00%20Geo%20France/FR_00%20Geo%20France%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>
## quest_start

<div class="yarn-node" data-title="quest_start"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// fr_00 | France GEO</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags: Start</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $COUNTRIES_COMPLETED = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $TOTAL_COUNTRIES = 7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $france_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $france_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $germany_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $germany_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $italy_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $italy_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $swiss_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $swiss_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $lux_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $lux_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $belgium_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $belgium_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $spain_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $spain_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action area_small&gt;&gt;</span>

<span class="yarn-line">Welcome to France! <span class="yarn-meta">#line:046db1f </span></span>
<span class="yarn-line">We are in Europe. <span class="yarn-meta">#line:08a8ce8 </span></span>
<span class="yarn-line">Let’s meet kids from nearby countries. <span class="yarn-meta">#line:08a09de </span></span>


</code></pre></div>

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The quest is complete. <span class="yarn-meta">#line:0432689 </span></span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Can you draw your flag now? <span class="yarn-meta">#line:0c48a14 </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-win"></a>
## win

<div class="yarn-node" data-title="win"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// FRANCE</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">tags: actor=KID_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Good job! You did it! <span class="yarn-meta">#line:0ba3c4c </span></span>
<span class="yarn-cmd">&lt;&lt;card concept_europe_map&gt;&gt;</span>
<span class="yarn-line">You found part of Europe! <span class="yarn-meta">#line:06e1dd4 </span></span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-france-npc"></a>
## france_npc

<div class="yarn-node" data-title="france_npc"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: france</span>
<span class="yarn-header-dim">tags: actor=KID_F</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $COUNTRIES_COMPLETED &gt;= $TOTAL_COUNTRIES&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;jump win&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $france_completed&gt;&gt;</span>
<span class="yarn-line">    Thank you for helping me! <span class="yarn-meta">#line:04e2d8b </span></span>
<span class="yarn-line">    Can you help my other friends? <span class="yarn-meta">#line:0e426ba </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_france"&gt;&gt;</span>
<span class="yarn-line">    Yes, that is my flag! Merci! <span class="yarn-meta">#line:01e24a8 </span></span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_france remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_FRENCH_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $france_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
<span class="yarn-line">    Can you find the German flag? <span class="yarn-meta">#line:04bd4db </span></span>
    <span class="yarn-cmd">&lt;&lt;set $france_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action germany_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action area_bigger&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_germany  &gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    That’s not my flag. Mine is blue, white, and red. <span class="yarn-meta">#line:04e0432 </span></span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Hello! I’m from France! <span class="yarn-meta">#line:06dfdd6 </span></span>
<span class="yarn-line">    Antura mixed up all the flags! <span class="yarn-meta">#line:0a20eed </span></span>
<span class="yarn-line">    My flag is blue, white, and red. <span class="yarn-meta">#line:0868737 </span></span>
<span class="yarn-line">    Can you help me? <span class="yarn-meta">#line:0a9f34e </span></span>
    <span class="yarn-cmd">&lt;&lt;set $france_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_france&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-france"></a>
## task_france

<div class="yarn-node" data-title="task_france"><pre class="yarn-code"><code><span class="yarn-header-dim">group: france</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset flag_france&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the French flag. <span class="yarn-meta">#line:0e35434 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_FRENCH_FLAG task_france&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-france"></a>
## item_flag_france

<div class="yarn-node" data-title="item_flag_france"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: france</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, asset=flag_france</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_france&gt;&gt;</span>
<span class="yarn-line">Flag of France. <span class="yarn-meta">#line:01d9617 </span></span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_france"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_france add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-germany-npc"></a>
## germany_npc

<div class="yarn-node" data-title="germany_npc"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// GERMANY</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: germany</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $germany_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_germany&gt;&gt;</span>
<span class="yarn-line">    Thank you for helping me! <span class="yarn-meta">#line:0eaf07d </span></span>
<span class="yarn-line">    Berlin is the capital of Germany. <span class="yarn-meta">#line:0446f03 </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_germany"&gt;&gt;</span>
<span class="yarn-line">    Danke! That is my flag! <span class="yarn-meta">#line:0ba8707</span></span>
<span class="yarn-line">    Can you help my Spanish friend? <span class="yarn-meta">#line:03684cb </span></span>
    <span class="yarn-cmd">&lt;&lt;set $germany_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_germany remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_GERMAN_FLAG &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action spain_active&gt;&gt;</span>
     <span class="yarn-cmd">&lt;&lt;set $germany_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_spain&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Our flag has stripes: black, red, yellow. <span class="yarn-meta">#line:0cd7024 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $germany_met == false &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $germany_met = true&gt;&gt;</span>
<span class="yarn-line">    Hello! I’m from Germany! <span class="yarn-meta">#line:0068fe1 </span></span>
<span class="yarn-line">    We're famous for castles, forests, and trains! <span class="yarn-meta">#line:04dc97a </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-germany"></a>
## task_germany

<div class="yarn-node" data-title="task_germany"><pre class="yarn-code"><code><span class="yarn-header-dim">group: germany</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the German flag and bring it back. <span class="yarn-meta">#line:029ee72 </span></span>
<span class="yarn-line">It has stripes: black, red, yellow. <span class="yarn-meta">#line:0f95ef2 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_GERMAN_FLAG task_germany&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-spain-npc"></a>
## spain_npc

<div class="yarn-node" data-title="spain_npc"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// SPAIN</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: spain</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $spain_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_spain&gt;&gt;</span>
<span class="yarn-line">    Thank you for helping me! <span class="yarn-meta">#line:0a5c214 </span></span>
<span class="yarn-line">    Barcelona and Madrid are big cities in Spain. <span class="yarn-meta">#line:09cf6c9 </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_spain"&gt;&gt;</span>
<span class="yarn-line">    That is my flag! <span class="yarn-meta">#line:0c57e40 </span></span>
<span class="yarn-line">    Thank you. Can you give my Italian friend their flag? <span class="yarn-meta">#line:0930602 </span></span>
    <span class="yarn-cmd">&lt;&lt;set $spain_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_spain remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_SPANISH_FLAG &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action italy_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $spain_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_italy&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Not mine! Our flag is red and yellow. <span class="yarn-meta">#line:0db05da </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_spain&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $spain_met == false &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $spain_met = true&gt;&gt;</span>
<span class="yarn-line">    Hola! I’m from Spain! <span class="yarn-meta">#line:0f5bc06 </span></span>
<span class="yarn-line">    We have a dance called flamenco! <span class="yarn-meta">#line:05e6d48 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_spain&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-task-spain"></a>
## task_spain

<div class="yarn-node" data-title="task_spain"><pre class="yarn-code"><code><span class="yarn-header-dim">group: spain</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_spain&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the Spanish flag. <span class="yarn-meta">#line:091cc7c </span></span>
<span class="yarn-line">It is red and yellow like the sun. <span class="yarn-meta">#line:09635b4 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_SPANISH_FLAG task_spain&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-italy-npc"></a>
## italy_npc

<div class="yarn-node" data-title="italy_npc"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// ITALY</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: italy</span>
<span class="yarn-header-dim">tags: actor=KID_M</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $italy_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_italy&gt;&gt;</span>
<span class="yarn-line">    Thank you! Our capital is Rome! <span class="yarn-meta">#line:0148edb </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_italy"&gt;&gt;</span>
<span class="yarn-line">    Grazie! That's my flag! <span class="yarn-meta">#line:081ac66 </span></span>
<span class="yarn-line">    Help them find the Belgian flag! <span class="yarn-meta">#line:001f54f </span></span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_italy remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_ITALIAN_FLAG &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $italy_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action belgium_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_belgium&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    My flag is green, white, and red. <span class="yarn-meta">#line:0dc8623</span></span>
    <span class="yarn-cmd">&lt;&lt;set $italy_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_italy&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $italy_met == false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $italy_met = true&gt;&gt;</span>
<span class="yarn-line">    Ciao! I'm from Italy! <span class="yarn-meta">#line:0bda0fc </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_italy&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-italy"></a>
## task_italy

<div class="yarn-node" data-title="task_italy"><pre class="yarn-code"><code><span class="yarn-header-dim">group: italy</span>
<span class="yarn-header-dim">tags: actor=KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_italy&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the Italian flag. <span class="yarn-meta">#line:0ed29f1 </span></span>
<span class="yarn-line">It is green, white, and red like a pizza! <span class="yarn-meta">#line:0cde44c </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_ITALIAN_FLAG task_italy&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-belgium-npc"></a>
## belgium_npc

<div class="yarn-node" data-title="belgium_npc"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// BELGIUM</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: belgium</span>
<span class="yarn-header-dim">tags: actor=WOMAN_OLD</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $belgium_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_belgium&gt;&gt;</span>
<span class="yarn-line">    Thank you for helping us! <span class="yarn-meta">#line:080a099 </span></span>
<span class="yarn-line">    Brussels is the capital of Belgium. <span class="yarn-meta">#line:06b3ceb </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_belgium"&gt;&gt;</span>
<span class="yarn-line">    Thank you! My flag is back! <span class="yarn-meta">#line:079096a </span></span>
<span class="yarn-line">    Can you help my Luxembourg friend? <span class="yarn-meta">#line:03a9b7d </span></span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_belgium remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_BELGIUM_FLAG &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belgium_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action lux_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belgium_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_lux&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Remember: my flag has stripes: black, yellow, red. <span class="yarn-meta">#line:0141a13 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_belgium&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belgium_met = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belgium_met = true&gt;&gt;</span>
<span class="yarn-line">    Bonjour! I'm from Belgium! <span class="yarn-meta">#line:0a61b67 </span></span>
<span class="yarn-line">    We also speak French! <span class="yarn-meta">#line:0b18893 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_belgium&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-belgium"></a>
## task_belgium

<div class="yarn-node" data-title="task_belgium"><pre class="yarn-code"><code><span class="yarn-header-dim">group: belgium</span>
<span class="yarn-header-dim">tags: actor=WOMAN_OLD</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_belgium&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the Belgian flag. <span class="yarn-meta">#line:0f08126 </span></span>
<span class="yarn-line">It's black, yellow, and red with vertical stripes. <span class="yarn-meta">#line:079ecca </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_BELGIUM_FLAG task_belgium&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-belgium"></a>
## item_flag_belgium

<div class="yarn-node" data-title="item_flag_belgium"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: belgium</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_belgium&gt;&gt;</span>
<span class="yarn-line">Flag of Belgium. <span class="yarn-meta">#line:0b11066 </span></span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_belgium"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_belgium add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-luxembourg-npc"></a>
## luxembourg_npc

<div class="yarn-node" data-title="luxembourg_npc"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// LUXEMBURG</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: lux</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags: actor=MAN_OLD</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $lux_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_luxembourg&gt;&gt;</span>
<span class="yarn-line">    Thank you for helping us! <span class="yarn-meta">#line:02114ba </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_luxembourg"&gt;&gt;</span>
<span class="yarn-line">    Thank you! That is my flag. <span class="yarn-meta">#line:05de5ab </span></span>
<span class="yarn-line">    Can you help my Swiss friend? <span class="yarn-meta">#line:0090192 </span></span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_luxembourg remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_LUX_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $lux_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action swiss_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_swiss&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Nope! Our flag is red, white, and light blue. <span class="yarn-meta">#line:0529472 </span></span>
    <span class="yarn-cmd">&lt;&lt;set $lux_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_lux&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $lux_met = true&gt;&gt;</span>
<span class="yarn-line">    Moien! I’m from Luxembourg! <span class="yarn-meta">#line:0b22d58 </span></span>
<span class="yarn-line">    We are small and speak three languages! <span class="yarn-meta">#line:0a3d0e3 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_lux&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-lux"></a>
## task_lux

<div class="yarn-node" data-title="task_lux"><pre class="yarn-code"><code><span class="yarn-header-dim">group: lux</span>
<span class="yarn-header-dim">tags: actor=WOMAN_OLD</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_luxembourg&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the Luxembourg flag. <span class="yarn-meta">#line:0ed3698 </span></span>
<span class="yarn-line">It is red, white, and light blue. <span class="yarn-meta">#line:018e4cf </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_LUX_FLAG task_lux&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-lux"></a>
## item_flag_lux

<div class="yarn-node" data-title="item_flag_lux"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: lux</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_luxembourg&gt;&gt;</span>
<span class="yarn-line">Flag of Luxembourg. <span class="yarn-meta">#line:05badd7 </span></span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_luxembourg"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_luxembourg add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-npc-swiss"></a>
## npc_swiss

<div class="yarn-node" data-title="npc_swiss"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// SWISS</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: swiss</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $swiss_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_switzerland&gt;&gt;</span>
<span class="yarn-line">    Thank you for helping us! <span class="yarn-meta">#line:02f73c8 </span></span>
<span class="yarn-line">    The capital of Switzerland is Bern! <span class="yarn-meta">#line:0d2e2d7 </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_switzerland"&gt;&gt;</span>
<span class="yarn-line">    Thank you for bringing my flag back! <span class="yarn-meta">#line:0ca99a0 </span></span>
<span class="yarn-line">    Go back to the start and get your prize! <span class="yarn-meta">#line:0148420 </span></span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_switzerland remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_SWISS_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $swiss_completed = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Not my flag. Our flag is red with a white cross. <span class="yarn-meta">#line:0caad5a </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_swiss&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $swiss_met = true&gt;&gt;</span>
<span class="yarn-line">    Hello! I’m from Switzerland! <span class="yarn-meta">#line:09fe8fc </span></span>
<span class="yarn-line">    We're famous for snowy mountains and cheese. <span class="yarn-meta">#line:0bf6b0d </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_swiss&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-task-swiss"></a>
## task_swiss

<div class="yarn-node" data-title="task_swiss"><pre class="yarn-code"><code><span class="yarn-header-dim">group: swiss</span>
<span class="yarn-header-dim">tags: actor=WOMAN_OLD</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_switzerland&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the Swiss flag. <span class="yarn-meta">#line:0ec7096 </span></span>
<span class="yarn-line">It is red with a big white cross like a first aid kit. <span class="yarn-meta">#line:07cc57e </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_SWISS_FLAG npc_swiss&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-monaco"></a>
## monaco

<div class="yarn-node" data-title="monaco"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// MONACO</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: monaco</span>
<span class="yarn-header-dim">tags: actor=MAN_BIG</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour! I’m from Monaco! <span class="yarn-meta">#line:0a3f8f6 </span></span>
<span class="yarn-line">We are small. We have car races and a palace by the sea! <span class="yarn-meta">#line:0dc315a </span></span>
<span class="yarn-cmd">&lt;&lt;asset flag_monaco&gt;&gt;</span>
<span class="yarn-line">My flag is red and white. <span class="yarn-meta">#line:00bd939 </span></span>

</code></pre></div>

<a id="ys-node-andorra"></a>
## andorra

<div class="yarn-node" data-title="andorra"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// ANDORRA</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: andorra</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hola! I’m from Andorra! <span class="yarn-meta">#line:071f85c </span></span>
<span class="yarn-line">My flag is blue, yellow, and red. <span class="yarn-meta">#line:02846e8 </span></span>
<span class="yarn-cmd">&lt;&lt;asset flag_andorra&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-item-flag-spain"></a>
## item_flag_spain

<div class="yarn-node" data-title="item_flag_spain"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">group: spain</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  flag_spain&gt;&gt;</span>
<span class="yarn-line">Flag of Spain <span class="yarn-meta">#line:006ce10 </span></span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_spain"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_spain add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-germany"></a>
## item_flag_germany

<div class="yarn-node" data-title="item_flag_germany"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: germany</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  flag_germany&gt;&gt;</span>
<span class="yarn-line">Flag of Germany. <span class="yarn-meta">#line:05ff51a </span></span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_germany"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_germany add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-italy"></a>
## item_flag_italy

<div class="yarn-node" data-title="item_flag_italy"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: italy</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_italy&gt;&gt;</span>
<span class="yarn-line">Flag of Italy. <span class="yarn-meta">#line:0fdc68b </span></span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_italy"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_italy add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-switzerland"></a>
## item_flag_switzerland

<div class="yarn-node" data-title="item_flag_switzerland"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: swiss</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  flag_switzerland&gt;&gt;</span>
<span class="yarn-line">Flag of Switzerland. <span class="yarn-meta">#line:0768ab7 </span></span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_switzerland"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_switzerland add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-spawned-npc"></a>
## spawned_npc

<div class="yarn-node" data-title="spawned_npc"><pre class="yarn-code"><code><span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: all </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">In Europe, people speak over 200 languages! <span class="yarn-meta">#line:04f4280 </span></span>
<span class="yarn-line">You can visit many of these countries without a passport! <span class="yarn-meta">#line:08d624d </span></span>
<span class="yarn-line">France is the biggest country in the European Union. <span class="yarn-meta">#line:0ab3dd9 </span></span>

</code></pre></div>


