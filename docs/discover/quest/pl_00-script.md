---
title: The neighbors of Poland (pl_00) - Script
hide:
---

# The neighbors of Poland (pl_00) - Script
[Quest Index](./index.md) - Language: english - [french](./pl_00-script.fr.md) - [polish](./pl_00-script.pl.md) - [italian](./pl_00-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/pl-00-the-neighbors-of-poland/31/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1233127135#gid=1233127135)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_00%20Geo%20Poland/PL_00%20Geo%20Poland%20-%20Yarn%20Script.yarn)  

<div class="yarn-node"><pre class="yarn-code"><code><span class="yarn-header-dim">// PL_00 GEOGRAPHY - Polish Geography Overview</span>
<span class="yarn-header-dim">// </span>
</code></pre></div>

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">=</span>
<span class="yarn-header-dim">// Words used: Poland, geography, Vistula, Odra, Tatra, Baltic Sea, Warsaw, Krakow, Wroclaw, Gdansk, mountains, rivers, coast, regions, cities</span>
<span class="yarn-header-dim">tags: Start</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $poland_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $poland_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $germany_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $germany_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $belarus_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $belarus_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $ukraine_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $ukraine_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $slovakia_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $slovakia_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $lithuania_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $lithuania_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $czech_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $czech_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action area_small&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-win"></a>
## win

<div class="yarn-node" data-title="win"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// poland</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">tags: actor=KID_FEMALE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Good job! You did it! <span class="yarn-meta">#line:0ba3c4c </span></span>
<span class="yarn-line">You discovered a part of Central Euorpe!</span>
<span class="yarn-cmd">&lt;&lt;card concept_europe_map&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-poland-npc"></a>
## poland_npc

<div class="yarn-node" data-title="poland_npc"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: poland</span>
<span class="yarn-header-dim">tags: actor=KID_FEMALE</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $CURRENT_PROGRESS &gt;= $MAX_PROGRESS&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;jump win&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $poland_completed == true&gt;&gt;</span>
<span class="yarn-line">    Thank you for helping me!</span>
<span class="yarn-line">    Can you help my other friends?</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_poland"&gt;&gt;</span>
<span class="yarn-line">    Yes, that is my flag! Thank you! <span class="yarn-meta">#line:0c9fa89 </span></span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_poland remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_polish_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $poland_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
<span class="yarn-line">    Can you find the German Flag? <span class="yarn-meta">#line:04bd4db </span></span>
    <span class="yarn-cmd">&lt;&lt;set $poland_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action germany_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action area_bigger&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_germany  &gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    That's not my flag. Mine is white and red. <span class="yarn-meta">#line:0f967da </span></span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Hello! I am from Poland!  <span class="yarn-meta">#line:017ad80 </span></span>
<span class="yarn-line">    Antura made a mess and all the flags have been mixed up! <span class="yarn-meta">#line:0b9e31d </span></span>
<span class="yarn-line">    My flag, the polish one is white and red.  <span class="yarn-meta">#line:08a2f6d </span></span>
<span class="yarn-line">    Can you help me?  <span class="yarn-meta">#line:0fe57ef </span></span>
    <span class="yarn-cmd">&lt;&lt;set $poland_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_poland&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-poland"></a>
## task_poland

<div class="yarn-node" data-title="task_poland"><pre class="yarn-code"><code><span class="yarn-header-dim">group: poland</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;card flag_poland&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the polish flag. <span class="yarn-meta">#line:09e3b54 </span></span>
<span class="yarn-line">It's white and red. <span class="yarn-meta">#line:0b52ba1 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_polish_FLAG task_poland&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-poland"></a>
## item_flag_poland

<div class="yarn-node" data-title="item_flag_poland"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: poland</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, asset=flag_poland</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_poland&gt;&gt;</span>
<span class="yarn-line">Flag of Poland.  <span class="yarn-meta">#line:07ca581 </span></span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_poland"&gt;&gt;</span>
    
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_poland add&gt;&gt;</span>
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
<span class="yarn-line">    Can you help my belarusian friend? <span class="yarn-meta">#line:06c463a </span></span>
    <span class="yarn-cmd">&lt;&lt;set $germany_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_germany remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_GERMAN_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action belarus_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $germany_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_belarus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    MAN: Our flag has horizontal stripes of black, red, and yellow. <span class="yarn-meta">#line:0cd7024 </span></span>
        <span class="yarn-cmd">&lt;&lt;jump task_germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $germany_met == false &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $germany_met = true&gt;&gt;</span>
<span class="yarn-line">    Hallo! I'm from Germany! <span class="yarn-meta">#line:0068fe1 </span></span>
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
<span class="yarn-line">Find the German flag and bring it to the German person. <span class="yarn-meta">#line:029ee72 </span></span>
<span class="yarn-line">It has horizontal stripes of black, red, and yellow. <span class="yarn-meta">#line:0f95ef2 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_GERMAN_FLAG task_germany&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-belarus-npc"></a>
## belarus_npc

<div class="yarn-node" data-title="belarus_npc"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// belarus</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: belarus</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $belarus_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_belarus&gt;&gt;</span>
<span class="yarn-line">    Thank you for helping me!  <span class="yarn-meta">#line:0a5c214 </span></span>
<span class="yarn-line">    Minsk is the capital of Belarus!  <span class="yarn-meta">#line:0aecb59 </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_belarus"&gt;&gt;</span>
<span class="yarn-line">    That is my flag! <span class="yarn-meta">#line:0c57e40 </span></span>
<span class="yarn-line">    Thank you, can you give my czech friend their flag? <span class="yarn-meta">#line:021e1a2 </span></span>
    <span class="yarn-cmd">&lt;&lt;set $belarus_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_belarus remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_belarusian_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action czech_republic_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belarus_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_czech_republic&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">        My flag is red and green with a red pattern on the left. <span class="yarn-meta">#line:0653fae </span></span>
        <span class="yarn-cmd">&lt;&lt;jump task_belarus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $belarus_met == false &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belarus_met = true&gt;&gt;</span>
<span class="yarn-line">    Hallo! I'm from Belarus!   <span class="yarn-meta">#line:0ccf58d </span></span>
<span class="yarn-line">    We have a primeaval forest, it has been growing for centuries!   <span class="yarn-meta">#line:0c6ac62 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_belarus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-task-belarus"></a>
## task_belarus

<div class="yarn-node" data-title="task_belarus"><pre class="yarn-code"><code><span class="yarn-header-dim">group: belarus</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_belarus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the belarusian flag.  <span class="yarn-meta">#line:0c00afe </span></span>
<span class="yarn-line">It's red and green, with a pattern on the left!  <span class="yarn-meta">#line:05c6081 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_belarusian_FLAG task_belarus&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-czech-republic-npc"></a>
## czech_republic_npc

<div class="yarn-node" data-title="czech_republic_npc"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// czech_republic</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: czech_republic</span>
<span class="yarn-header-dim">tags: actor=KID_MALE</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $czech_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_czech_republic&gt;&gt;</span>
<span class="yarn-line">    Thank you! Our capital is Minsk!   <span class="yarn-meta">#line:08473de </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_czech_republic"&gt;&gt;</span>
<span class="yarn-line">    Thank you! That's my flag!  <span class="yarn-meta">#line:07ba10f </span></span>
<span class="yarn-line">    Help the find the lithuanian flag and bring it to them!  <span class="yarn-meta">#line:0a1e0a3 </span></span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_czech_republic remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_czech_republic_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $czech_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action lithuania_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_lithuania&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    My flag is different! It's white and red with a blue triangle. <span class="yarn-meta">#line:0000741 </span></span>
    <span class="yarn-cmd">&lt;&lt;set $czech_met = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_czech_republic&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $czech_met == false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $czech_met = true&gt;&gt;</span>
<span class="yarn-line">    Hi! I'm from Czech Republic! <span class="yarn-meta">#line:0ded863 </span></span>
<span class="yarn-line">    We have the most castle in Europe! <span class="yarn-meta">#line:03dc09c </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_czech_republic&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-czech-republic"></a>
## task_czech_republic

<div class="yarn-node" data-title="task_czech_republic"><pre class="yarn-code"><code><span class="yarn-header-dim">group: czech_republic</span>
<span class="yarn-header-dim">tags: actor=KID_MALE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_czech_republic&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the Czech flag.  <span class="yarn-meta">#line:0ff23aa </span></span>
<span class="yarn-line">It's white and red with a blue triangle. <span class="yarn-meta">#line:03cb303 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_czech_republic_FLAG task_czech_republic&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-lithuania-npc"></a>
## lithuania_npc

<div class="yarn-node" data-title="lithuania_npc"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// lithuania</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: lithuania</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $lithuania_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_lithuania&gt;&gt;</span>
<span class="yarn-line">    Thank you for helping us! <span class="yarn-meta">#line:06cf019 </span></span>
<span class="yarn-line">    Brussels is the capital of Lithuania.  <span class="yarn-meta">#line:0acbb04 </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_lithuania"&gt;&gt;</span>
<span class="yarn-line">        Thank you, my beautiful flag is back!  <span class="yarn-meta">#line:0d2f54c </span></span>
<span class="yarn-line">        Can you help my Ukrainian friend? <span class="yarn-meta">#line:0bcf83b </span></span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_lithuania remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_lithuania_FLAG &gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $lithuania_completed = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action ukraine_active&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $lithuania_met = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_ukraine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Remember, my flag is red, green and yellow. <span class="yarn-meta">#line:00af906 </span></span>
        <span class="yarn-cmd">&lt;&lt;jump task_lithuania&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $lithuania_met = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $lithuania_met = true&gt;&gt;</span>
<span class="yarn-line">    Hi! I'm from Lithuania!  <span class="yarn-meta">#line:0534a85 </span></span>
<span class="yarn-line">    One third of our country is covered by forests!  <span class="yarn-meta">#line:0465c31 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_lithuania&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-lithuania"></a>
## task_lithuania

<div class="yarn-node" data-title="task_lithuania"><pre class="yarn-code"><code><span class="yarn-header-dim">group: lithuania</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_lithuania&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the Lithuanian flag.  <span class="yarn-meta">#line:0b88326 </span></span>
<span class="yarn-line">It's red, green and yellow. <span class="yarn-meta">#line:0754062 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_lithuania_FLAG task_lithuania&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-lithuania"></a>
## item_flag_lithuania

<div class="yarn-node" data-title="item_flag_lithuania"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: lithuania</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_lithuania&gt;&gt;</span>
<span class="yarn-line">Flag of Lithuania.  <span class="yarn-meta">#line:0942331 </span></span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_lithuania"&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_lithuania add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-ukraine-npc"></a>
## ukraine_npc

<div class="yarn-node" data-title="ukraine_npc"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// ukraineEMBURG</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: ukraine</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags: actor=OLD_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $ukraine_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_ukraine&gt;&gt;</span>
<span class="yarn-line">    Thank you for helping us! <span class="yarn-meta">#line:02114ba </span></span>
<span class="yarn-line">    Our capital is Kiev. <span class="yarn-meta">#line:01b1e6d </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_ukraine"&gt;&gt;</span>
<span class="yarn-line">        Thank you! That is my flag. <span class="yarn-meta">#line:05de5ab </span></span>
<span class="yarn-line">        Can you help my slovakian friend? <span class="yarn-meta">#line:0aa87ef </span></span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_ukraine remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_ukraine_FLAG&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $ukraine_completed = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action slovakia_active&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_slovakia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Nope! Our flag is blue and yellow. <span class="yarn-meta">#line:0a94866 </span></span>
    <span class="yarn-cmd">&lt;&lt;set $ukraine_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_ukraine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $ukraine_met = true&gt;&gt;</span>
<span class="yarn-line">    Moien! I’m from Ukraine! <span class="yarn-meta">#line:07ea99a </span></span>
<span class="yarn-line">    We are called the "Bread Basket" of Europe because we produce a lot of grain! <span class="yarn-meta">#line:0dbd4af </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_ukraine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-ukraine"></a>
## task_ukraine

<div class="yarn-node" data-title="task_ukraine"><pre class="yarn-code"><code><span class="yarn-header-dim">group: ukraine</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_ukraine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the flag of Ukraine.   <span class="yarn-meta">#line:07c148b </span></span>
<span class="yarn-line">It is blue and yellow. <span class="yarn-meta">#line:0b6e8e7 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_ukraine_FLAG task_ukraine&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-ukraine"></a>
## item_flag_ukraine

<div class="yarn-node" data-title="item_flag_ukraine"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: ukraine</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_ukraine&gt;&gt;</span>
<span class="yarn-line">Flag of Ukraine.  <span class="yarn-meta">#line:0805b90 </span></span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_ukraine"&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_ukraine add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-npc-slovakia"></a>
## npc_slovakia

<div class="yarn-node" data-title="npc_slovakia"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// slovakia</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: slovakia</span>
<span class="yarn-header-dim">tags: actor=CRAZY_WOMAN</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $slovakia_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_slovakia&gt;&gt;</span>
<span class="yarn-line">    Thank you for helping us!  <span class="yarn-meta">#line:06a6231 </span></span>
<span class="yarn-line">    The capital of Slovakia is Bratislava!  <span class="yarn-meta">#line:0891aba </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_slovakia"&gt;&gt;</span>
<span class="yarn-line">        Thank you for bringing my flag back!  <span class="yarn-meta">#line:0d453e9 </span></span>
<span class="yarn-line">        Go back to the start and claim your victory! <span class="yarn-meta">#line:04bf6d1 </span></span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_slovakia remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_slovakia_FLAG &gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $slovakia_completed = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">        Our flag is white, red and blue with a coagt of arms. <span class="yarn-meta">#line:0af30a1 </span></span>
        <span class="yarn-cmd">&lt;&lt;jump task_slovakia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $slovakia_met = true&gt;&gt;</span>
<span class="yarn-line">    I'm from Slovakia! <span class="yarn-meta">#line:054e1b8 </span></span>
<span class="yarn-line">    We used to be a country with Czech Republic.  <span class="yarn-meta">#line:0278f6a </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_slovakia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-task-slovakia"></a>
## task_slovakia

<div class="yarn-node" data-title="task_slovakia"><pre class="yarn-code"><code><span class="yarn-header-dim">group: slovakia</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_slovakia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the Slovakian flag.   <span class="yarn-meta">#line:04b6692 </span></span>
<span class="yarn-line">It's white, red and blue with the coat of arms. <span class="yarn-meta">#line:0866ee1 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_slovakia_FLAG npc_slovakia&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-russia"></a>
## russia

<div class="yarn-node" data-title="russia"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// Russia</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: russia</span>
<span class="yarn-header-dim">tags: actor=CRAZY_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">CRAZY_MAN: Hello, I'm Russia. <span class="yarn-meta">#line:065c41c </span></span>
<span class="yarn-line">CRAZY_MAN: This is just a tiny part of the big country. <span class="yarn-meta">#line:0a4bae4 </span></span>
<span class="yarn-cmd">&lt;&lt;card flag_russia&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-item-flag-belarus"></a>
## item_flag_belarus

<div class="yarn-node" data-title="item_flag_belarus"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">group: belarus</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_belarus&gt;&gt;</span>
<span class="yarn-line">Flag of belarus <span class="yarn-meta">#line:006ce10 </span></span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_belarus"&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_belarus add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-germany"></a>
## item_flag_germany

<div class="yarn-node" data-title="item_flag_germany"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: germany</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_germany&gt;&gt;</span>
<span class="yarn-line">Flag of Germany. <span class="yarn-meta">#line:05ff51a </span></span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_germany"&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_germany add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-czech-republic"></a>
## item_flag_czech_republic

<div class="yarn-node" data-title="item_flag_czech_republic"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: czech_republic</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_czech_republic&gt;&gt;</span>
<span class="yarn-line">Flag of Czech Republic. <span class="yarn-meta">#line:0fdc68b </span></span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_czech_republic"&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_czech_republic add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-slovakia"></a>
## item_flag_slovakia

<div class="yarn-node" data-title="item_flag_slovakia"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: slovakia</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_slovakia&gt;&gt;</span>
<span class="yarn-line">Flag of slovakia. <span class="yarn-meta">#line:0768ab7 </span></span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_slovakia"&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_slovakia add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>


