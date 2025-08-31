---
title: Sąsiedzi Polski (pl_00) - Script
hide:
---

# Sąsiedzi Polski (pl_00) - Script
[Quest Index](./index.pl.md) - Language: [english](./pl_00-script.md) - [french](./pl_00-script.fr.md) - polish - [italian](./pl_00-script.it.md)

<div class="yarn-node"><pre class="yarn-code"><code><span class="yarn-header-dim">// </span>
</code></pre></div>

<div class="yarn-node"><pre class="yarn-code"><code><span class="yarn-header-dim">=</span>
<span class="yarn-header-dim">// PL_00_GEOGRAPHY - Polish Geography Overview</span>
<span class="yarn-header-dim">// </span>
</code></pre></div>

<div class="yarn-node"><pre class="yarn-code"><code><span class="yarn-header-dim">=</span>
<span class="yarn-header-dim">//</span>
<span class="yarn-header-dim">// Cards:</span>
<span class="yarn-header-dim">// - poland_map (geographical overview)</span>
<span class="yarn-header-dim">//</span>
<span class="yarn-header-dim">// Tasks:</span>
<span class="yarn-header-dim">//</span>
<span class="yarn-header-dim">// Activities:</span>
<span class="yarn-header-dim">//</span>
<span class="yarn-header-dim">// Words used: Poland, geography, Vistula, Odra, Tatra, Baltic Sea, Warsaw, Krakow, Wroclaw, Gdansk, mountains, rivers, coast, regions, cities</span>
<span class="yarn-header-dim">// </span>
</code></pre></div>

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">=</span>
<span class="yarn-header-dim">tags: Start</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = 0&gt;&gt;</span>
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


</code></pre></div>

<a id="ys-node-win"></a>
## win

<div class="yarn-node" data-title="win"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// Poland</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">tags: actor=KID_FEMALE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Good job! You did it! <span class="yarn-meta">#line:0246d71 </span></span>
    <span class="yarn-cmd">&lt;&lt;quest_end 3&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-poland-npc"></a>
## poland_npc

<div class="yarn-node" data-title="poland_npc"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: poland</span>
<span class="yarn-header-dim">tags: actor=KID_FEMALE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_of_poland"&gt;&gt;</span>
<span class="yarn-line">    Yes, that is my flag! <span class="yarn-meta">#line:004056b </span></span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_of_france remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $poland_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
<span class="yarn-line">    Can you bring the other flags to their respective country? Thank you! <span class="yarn-meta">#line:0fbf723 </span></span>
    <span class="yarn-cmd">&lt;&lt;action area_bigger&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    That's not my flag. Mine is blue, white, and red. <span class="yarn-meta">#line:074477d </span></span>
&lt;&lt;elseif $CURRENT_PROGRESS &gt;= $MAX_PROGRESS&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;jump win&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Hello! I am from Poland! <span class="yarn-meta">#line:004ac54 </span></span>
<span class="yarn-line">    Antura made a mess and all the flags have been mixed up! <span class="yarn-meta">#line:0cb2732 </span></span>
<span class="yarn-line">    My flag, the Polish one white, and red. <span class="yarn-meta">#line:03db34d </span></span>
<span class="yarn-line">    Can you help me? <span class="yarn-meta">#line:0a08fc8 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_poland&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-poland"></a>
## task_poland

<div class="yarn-node" data-title="task_poland"><pre class="yarn-code"><code><span class="yarn-header-dim">group: poland</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;card flag_of_poland&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the Polish flag. <span class="yarn-meta">#line:063d732 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_POLISH_FLAG&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-of-poland"></a>
## item_flag_of_poland

<div class="yarn-node" data-title="item_flag_of_poland"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: poland</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  flag_of_poland&gt;&gt;</span>
<span class="yarn-line">TUTOR: Flag of Poland. <span class="yarn-meta">#line:0909e55 </span></span>
<span class="yarn-cmd">&lt;&lt;inventory flag_of_poland add&gt;&gt;</span>

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
    <span class="yarn-cmd">&lt;&lt;asset flag_of_germany&gt;&gt;</span>
<span class="yarn-line">    Thank you for helping me! <span class="yarn-meta">#line:0ddf1ea </span></span>
<span class="yarn-line">    Berlin is the capital of Germany. <span class="yarn-meta">#line:076346f </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $germany_met&gt;&gt;</span>
<span class="yarn-line">    Choose the black, red and yellow horizontal flag. <span class="yarn-meta">#line:0b7e82e </span></span>
    <span class="yarn-cmd">&lt;&lt;inventory select flag&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_of_germany"&gt;&gt;</span>
<span class="yarn-line">        Danke! That is my flag! <span class="yarn-meta">#line:06c305e </span></span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_of_germany remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end germany&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $germany_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    MAN: Our flag has horizontal stripes of black, red, and yellow. <span class="yarn-meta">#line:046a2af </span></span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_germany&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $germany_met = true&gt;&gt;</span>
<span class="yarn-line">    Hallo! I'm from Germany! <span class="yarn-meta">#line:060ba97 </span></span>
<span class="yarn-line">    We're famous for castles, forests, and trains! <span class="yarn-meta">#line:0ba5308 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-germany"></a>
## task_germany

<div class="yarn-node" data-title="task_germany"><pre class="yarn-code"><code><span class="yarn-header-dim">group: germany</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_of_germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the German flag. <span class="yarn-meta">#line:0c76fa7 </span></span>
<span class="yarn-line">It has horizontal stripes of black, red, and yellow. <span class="yarn-meta">#line:009b752 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_GERMAN_FLAG&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-belarus-npc"></a>
## belarus_npc

<div class="yarn-node" data-title="belarus_npc"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// BELARUS</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: belarus</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $belarus_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset flag_of_belarus&gt;&gt;</span>
<span class="yarn-line">    Thank you! <span class="yarn-meta">#line:0517296 </span></span>
<span class="yarn-line">    The capital of Belarus is Minsk!. <span class="yarn-meta">#line:05ed8ab </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $belarus_met&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_of_belarus"&gt;&gt;</span>
<span class="yarn-line">        Gracias! <span class="yarn-meta">#line:01376bf </span></span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_of_belarus remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end belarus&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $belarus_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">        That is not my flag. Mine is red and green. <span class="yarn-meta">#line:0a319dd </span></span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_belarus&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belarus_met = true&gt;&gt;</span>
<span class="yarn-line">    Hi! I'm from Belarus! <span class="yarn-meta">#line:050ec0d </span></span>
<span class="yarn-line">    We have the last European primeval forest! <span class="yarn-meta">#line:0046d26 </span></span>
<span class="yarn-line">    That is a forest that has been growing for a long time! <span class="yarn-meta">#line:0fd7ede </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_belarus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-task-belarus"></a>
## task_belarus

<div class="yarn-node" data-title="task_belarus"><pre class="yarn-code"><code><span class="yarn-header-dim">group: belarus</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_of_belarus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the Belarus flag. <span class="yarn-meta">#line:05c18f8 </span></span>
<span class="yarn-line">It's red and green with a pattern on the left. <span class="yarn-meta">#line:0ee0bfb </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_BELARUS_FLAG&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-czech-npc"></a>
## czech_npc

<div class="yarn-node" data-title="czech_npc"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// CZECH REPUBLIC</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: czech</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags: actor=KID_MALE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $czech_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_of_czech_republic&gt;&gt;</span>
<span class="yarn-line">    Thank you! Our capital is Prague! <span class="yarn-meta">#line:0948c37 </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $czech_met&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_of_czech_republic"&gt;&gt;</span>
<span class="yarn-line">        Thank you! That's my flag! <span class="yarn-meta">#line:05f2561 </span></span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_of_czech_republic remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end czech&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $czech_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    My flag is different! It's blue, white, and red. <span class="yarn-meta">#line:02f60be </span></span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_czech&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $czech_met = true&gt;&gt;</span>
<span class="yarn-line">    Hello! I'm from Czech Republic! <span class="yarn-meta">#line:07ac39d </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_czech&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-czech"></a>
## task_czech

<div class="yarn-node" data-title="task_czech"><pre class="yarn-code"><code><span class="yarn-header-dim">group: czech</span>
<span class="yarn-header-dim">tags: actor=KID_MALE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_of_czech_republic&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the Czech Republic flag. <span class="yarn-meta">#line:0d56168 </span></span>
<span class="yarn-line">It's blue, white, and red! <span class="yarn-meta">#line:0d04c22 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_CZECH_FLAG&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-lithuania-npc"></a>
## lithuania_npc

<div class="yarn-node" data-title="lithuania_npc"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// LITHUANIA</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: lithuania</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $lithuania_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_of_lithuania&gt;&gt;</span>
<span class="yarn-line">    Thank you for helping us! <span class="yarn-meta">#line:00a65fb </span></span>
<span class="yarn-line">    Vilnius is the capital of Lithuania. <span class="yarn-meta">#line:055f101 </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $lithuania_met&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_of_lithuania"&gt;&gt;</span>
<span class="yarn-line">        Thank you, my beautiful flag is back! <span class="yarn-meta">#line:07aebc2 </span></span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_of_lithuania remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end lithuania&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $lithuania_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Remember, my flag is yellow, green and red. <span class="yarn-meta">#line:0ca0ee9 </span></span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_lithuania&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $lithuania_met = true&gt;&gt;</span>
<span class="yarn-line">    Hello! I'm from Lithuania. <span class="yarn-meta">#line:046809e </span></span>
<span class="yarn-line">    Like Poland we also border the Baltic Sea. <span class="yarn-meta">#line:0d1493d </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_lithuania&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-lithuania"></a>
## task_lithuania

<div class="yarn-node" data-title="task_lithuania"><pre class="yarn-code"><code><span class="yarn-header-dim">group: lithuania</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_of_lithuania&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the Lithuanian flag. <span class="yarn-meta">#line:0621460 </span></span>
<span class="yarn-line">It's green, yellow and red with horizontal stripes. <span class="yarn-meta">#line:0b0cacd </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_LITHUANIAN_FLAG&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-lithuania"></a>
## item_flag_lithuania

<div class="yarn-node" data-title="item_flag_lithuania"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: lithuania</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_of_lithuania&gt;&gt;</span>
<span class="yarn-line">TUTOR: Flag of Lithuania. <span class="yarn-meta">#line:03679c5 </span></span>
<span class="yarn-cmd">&lt;&lt;inventory flag_of_lithuania add&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-slovakia-npc"></a>
## slovakia_npc

<div class="yarn-node" data-title="slovakia_npc"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// SLOVAKIA</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: slovakia</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags: actor=OLD_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $slovakia_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset flag_of_slovakia&gt;&gt;</span>
<span class="yarn-line">    Thank you for helping us! <span class="yarn-meta">#line:01940d1 </span></span>
<span class="yarn-line">    The Slovakian capital is Bratislava! <span class="yarn-meta">#line:012daf5 </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $slovakia_met&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_of_slovakia"&gt;&gt;</span>
<span class="yarn-line">        Thank you! That is my flag. <span class="yarn-meta">#line:0532cfc </span></span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_of_slovakia add&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end slovakia&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $slovakia_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">        Nope! Our flag is white, blue and red with our coat of arms. <span class="yarn-meta">#line:06dfb2d </span></span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_slovakia&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $slovakia_met = true&gt;&gt;</span>
<span class="yarn-line">    Hello I’m from Slovakia! <span class="yarn-meta">#line:07817ee </span></span>
<span class="yarn-line">    We may be small, but we speak three languages! <span class="yarn-meta">#line:037ad41 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_slovakia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-slovakia"></a>
## task_slovakia

<div class="yarn-node" data-title="task_slovakia"><pre class="yarn-code"><code><span class="yarn-header-dim">group: slovakia</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_of_slovakia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the flag of Slovakia. <span class="yarn-meta">#line:0cec1f3 </span></span>
<span class="yarn-line">It is white, blue and red with their coat of arms on it. <span class="yarn-meta">#line:00324af </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_SLOVAKIA_FLAG&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-slovakia"></a>
## item_flag_slovakia

<div class="yarn-node" data-title="item_flag_slovakia"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: slovakia</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_of_slovakia&gt;&gt;</span>
<span class="yarn-line">TUTOR: Flag of Slovakia. <span class="yarn-meta">#line:01f9394 </span></span>
<span class="yarn-cmd">&lt;&lt;inventory flag_of_slovakia add&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-npc-ukraine"></a>
## npc_ukraine

<div class="yarn-node" data-title="npc_ukraine"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// UKRAINE</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: ukraine</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags: actor=CRAZY_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $ukraine_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_of_ukraine&gt;&gt;</span>
<span class="yarn-line">    Thank you for helping us! <span class="yarn-meta">#line:0d2cd81 </span></span>
<span class="yarn-line">    The capital of ukraine is Kiev! <span class="yarn-meta">#line:03a76f1 </span></span>
<span class="yarn-cmd">&lt;&lt;elseif $ukraine_met&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_of_ukraine"&gt;&gt;</span>
<span class="yarn-line">        Thank you for bringing my flag back! <span class="yarn-meta">#line:0701aad </span></span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_of_ukraine remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end ukraine&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $ukraine_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">        Not my flag. Our flag is yellow and blue. <span class="yarn-meta">#line:010f6b2 </span></span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_ukraine&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $ukraine_met = true&gt;&gt;</span>
<span class="yarn-line">    Hi I'm from Ukraine! <span class="yarn-meta">#line:060eb8d </span></span>
    <span class="yarn-cmd">&lt;&lt;jump task_ukraine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-ukraine"></a>
## task_ukraine

<div class="yarn-node" data-title="task_ukraine"><pre class="yarn-code"><code><span class="yarn-header-dim">group: ukraine</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $EASY_MODE == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card flag_of_ukraine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Find the Ukrainian flag. <span class="yarn-meta">#line:0ab0b16 </span></span>
<span class="yarn-line">It's blue and yellow! <span class="yarn-meta">#line:06fda57 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_UKRAINE_FLAG&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-russia"></a>
## russia

<div class="yarn-node" data-title="russia"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// MONACO</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: russia</span>
<span class="yarn-header-dim">tags: actor=CRAZY_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">CRAZY_MAN: Hello, I'm from Russia. <span class="yarn-meta">#line:0072d75 </span></span>
<span class="yarn-line">CRAZY_MAN: We’re a small part of a bigger country. <span class="yarn-meta">#line:06f0e39 </span></span>
<span class="yarn-cmd">&lt;&lt;card flag_of_russia&gt;&gt;</span>
<span class="yarn-line">CRAZY_MAN: My flag is white, blue and red <span class="yarn-meta">#line:04fa751 </span></span>


</code></pre></div>

<a id="ys-node-item-flag-belarus"></a>
## item_flag_belarus

<div class="yarn-node" data-title="item_flag_belarus"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">group: belarus</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  flag_of_belarus&gt;&gt;</span>
<span class="yarn-line">TUTOR: Flag of Belarus <span class="yarn-meta">#line:00c0390 </span></span>
<span class="yarn-cmd">&lt;&lt;inventory flag_of_belarus add&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-germany"></a>
## item_flag_germany

<div class="yarn-node" data-title="item_flag_germany"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: germany</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  flag_of_germany&gt;&gt;</span>
<span class="yarn-line">TUTOR: Flag of Germany. <span class="yarn-meta">#line:004b1d1 </span></span>
<span class="yarn-cmd">&lt;&lt;inventory flag_of_germany add&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-czech-republic"></a>
## item_flag_czech_republic

<div class="yarn-node" data-title="item_flag_czech_republic"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: czech</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_of_czech_republic&gt;&gt;</span>
<span class="yarn-line">TUTOR: Flag of Czech Republic. <span class="yarn-meta">#line:0613d96 </span></span>
<span class="yarn-cmd">&lt;&lt;inventory flag_of_czech_republic add&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-item-flag-ukraine"></a>
## item_flag_ukraine

<div class="yarn-node" data-title="item_flag_ukraine"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: ukraine</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  flag_of_ukraine&gt;&gt;</span>
<span class="yarn-line">TUTOR: Flag of Ukraine. <span class="yarn-meta">#line:0632e4c </span></span>
<span class="yarn-cmd">&lt;&lt;inventory flag_of_ukraine add&gt;&gt;</span>

</code></pre></div>


