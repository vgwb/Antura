---
title: Paris! (fr_01) - Script
hide:
---

# Paris! (fr_01) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-01-paris/23/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=755037318#gid=755037318)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_01%20Paris/FR_01%20Paris%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_01 | Paris</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $QUEST_ITEMS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $MET_GUIDE = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $MET_MAJOR = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $MET_MONALISA = false&gt;&gt;</span>

<span class="yarn-line">Welcome to Paris!</span> <span class="yarn-meta">#line:start </span>
<span class="yarn-line">Go talk to the tutor!</span> <span class="yarn-meta">#line:start_2</span>
<span class="yarn-cmd">&lt;&lt;target tutor&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great! I can bake the baguette now.</span> <span class="yarn-meta">#line:0017917 </span>
<span class="yarn-line">Congratulations! You won the game! Did you like it?</span> <span class="yarn-meta">#line:0d11596 </span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Why don't you draw the Eiffel Tower?</span> <span class="yarn-meta">#line:002620f </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-tutor"></a>

## talk_tutor

<div class="yarn-node" data-title="talk_tutor">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">actor: tutor</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I saw Antura go to the Eiffel Tower.</span> <span class="yarn-meta">#line:talk_tutor</span>
<span class="yarn-cmd">&lt;&lt;camera_focus tour_eiffell&gt;&gt;</span>
<span class="yarn-line">Follow the light or use the map!</span> <span class="yarn-meta">#line:talk_tutor_2 </span>
<span class="yarn-line">Go there now!</span> <span class="yarn-meta">#line:talk_tutor_3 </span>

</code>
</pre>
</div>

<a id="ys-node-talk-eiffell-roof"></a>

## talk_eiffell_roof

<div class="yarn-node" data-title="talk_eiffell_roof">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset toureiffell&gt;&gt;</span>
<span class="yarn-line">The Eiffel Tower is 300 meters tall.</span> <span class="yarn-meta">#line:08c1973 </span>
<span class="yarn-cmd">&lt;&lt;asset mr_eiffel&gt;&gt;</span>
<span class="yarn-line">Gustave Eiffel built it in 1887.</span> <span class="yarn-meta">#line:09e5c3b </span>
<span class="yarn-cmd">&lt;&lt;asset iron&gt;&gt;</span>
<span class="yarn-line">It is made of iron!</span> <span class="yarn-meta">#line:0d59ade </span>
<span class="yarn-line">I saw Antura go to Notre Dame.</span> <span class="yarn-meta">#line:04d1e52 </span>
<span class="yarn-cmd">&lt;&lt;camera_focus notredame&gt;&gt;</span>
<span class="yarn-line">Go there!</span> <span class="yarn-meta">#line:083b3bf </span>


</code>
</pre>
</div>

<a id="ys-node-talk-eiffell-guide"></a>

## talk_eiffell_guide

<div class="yarn-node" data-title="talk_eiffell_guide">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">actor: guide</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $TOTAL_COINS &gt; 2&gt;&gt;
<span class="yarn-line">    Here is your ticket.</span> <span class="yarn-meta">#line:04e74ad </span>
    <span class="yarn-cmd">&lt;&lt;asset tour_eiffell_ticket&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = $TOTAL_COINS-3&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action area_toureiffel&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset tour_eiffell_map&gt;&gt;</span>
<span class="yarn-line">    I saw Antura going up to the top of the tower.</span> <span class="yarn-meta">#line:089abda </span>
<span class="yarn-line">    Take the elevator!</span> <span class="yarn-meta">#line:0585a5e </span>
&lt;&lt;elseif $TOTAL_COINS &gt; 0&gt;&gt; 
<span class="yarn-line">    Collect all the coins!</span> <span class="yarn-meta">#line:04d966b </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    The ticket costs 3 coins.</span> <span class="yarn-meta">#line:069cbb3 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-notre-dame"></a>

## talk_notre_dame

<div class="yarn-node" data-title="talk_notre_dame">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">tags: actor=MAN_OLD, asset=notredame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I am the mayor of Paris.</span> <span class="yarn-meta">#line:0cc11fa </span>
<span class="yarn-line">This is Notre-Dame Cathedral.</span> <span class="yarn-meta">#line:06f3fa2 </span>
<span class="yarn-cmd">&lt;&lt;card notredame zoom&gt;&gt;</span>
<span class="yarn-line">It is a famous Gothic church. Built in 1182.</span> <span class="yarn-meta">#line:02edc0f </span>
<span class="yarn-cmd">&lt;&lt;action AREA_NOTREDAME_ROOF&gt;&gt;</span>
<span class="yarn-line">Come with me to the roof!</span> <span class="yarn-meta">#line:083dfcc </span>
<span class="yarn-cmd">&lt;&lt;set $MET_MAJOR = true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-notre-dame-roof"></a>

## talk_notre_dame_roof

<div class="yarn-node" data-title="talk_notre_dame_roof">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">tags: actor=MAN_OLD</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset notredame_fire&gt;&gt;</span>
<span class="yarn-line">There was a big fire in 2019, but we repaired it.</span> <span class="yarn-meta">#line:09a0ead </span>
<span class="yarn-line">I saw Antura run into the Louvre.</span> <span class="yarn-meta">#line:02ba888 </span>
<span class="yarn-line">It is across the River Seine.</span> <span class="yarn-meta">#line:00d22e5 </span>

</code>
</pre>
</div>

<a id="ys-node-gargoyle"></a>

## gargoyle

<div class="yarn-node" data-title="gargoyle">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Look at that statue!</span> <span class="yarn-meta">#line:0f7f9d8 </span>
<span class="yarn-cmd">&lt;&lt;card gargoyle zoom&gt;&gt;</span>
<span class="yarn-line">Isn't it scary?</span> <span class="yarn-meta">#line:0b5d057 </span>

</code>
</pre>
</div>

<a id="ys-node-talk-louvre-external"></a>

## talk_louvre_external

<div class="yarn-node" data-title="talk_louvre_external">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">actor: OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $MET_MONALISA&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card go_bakery&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;asset louvre&gt;&gt;</span>
<span class="yarn-line">This is the entrance to the Louvre, our national art museum.</span> <span class="yarn-meta">#line:0cf1cc8 </span>
<span class="yarn-line">Do you want to go in?</span> <span class="yarn-meta">#line:0f74ff9</span>
<span class="yarn-line">Yes</span> <span class="yarn-meta">#line:090114f </span>
<span class="yarn-line">    Enjoy your visit!</span> <span class="yarn-meta">#line:056e051 </span>
    <span class="yarn-cmd">&lt;&lt;action AREA_LOUVRE_ENTER &gt;&gt;</span>
<span class="yarn-line">No</span> <span class="yarn-meta">#line:077422a </span>
<span class="yarn-line">    Ok. Au revoir.</span> <span class="yarn-meta">#line:0c28ea0 #do_not_translate</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-louvre-guide"></a>

## talk_louvre_guide

<div class="yarn-node" data-title="talk_louvre_guide">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Welcome to the Louvre. What do you want to do?</span> <span class="yarn-meta">#line:0e6d2a5 </span>
<span class="yarn-line">Tell me about the Louvre</span> <span class="yarn-meta">#line:0a5fc63 </span>
    <span class="yarn-cmd">&lt;&lt;jump visit_louvre&gt;&gt;</span>
<span class="yarn-line">Exit</span> <span class="yarn-meta">#line:0efc18f </span>
    <span class="yarn-cmd">&lt;&lt;if $MET_MONALISA&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action AREA_LOUVRE_EXIT&gt;&gt;</span>
<span class="yarn-line">        Come back!</span> <span class="yarn-meta">#line:07dd921 </span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump find_monalisa&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-louvre-monalisa"></a>

## louvre_monalisa

<div class="yarn-node" data-title="louvre_monalisa">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset monalisa&gt;&gt;</span>
<span class="yarn-line">This is the famous Mona Lisa.</span> <span class="yarn-meta">#line:louvre_monalisa_1</span>
<span class="yarn-cmd">&lt;&lt;set $MET_MONALISA = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;asset leaonardodavinci&gt;&gt;</span>
<span class="yarn-line">Leonardo painted it around 1500.</span> <span class="yarn-meta">#line:louvre_monalisa_2</span>
<span class="yarn-line">by the artist Leonardo da Vinci.</span> <span class="yarn-meta">#line:louvre_monalisa_3</span>

</code>
</pre>
</div>

<a id="ys-node-louvre-liberty"></a>

## louvre_liberty

<div class="yarn-node" data-title="louvre_liberty">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset liberty_leading&gt;&gt;</span>
<span class="yarn-line">This painting represents freedom.</span> <span class="yarn-meta">#line:louvre_liberty_1</span>
<span class="yarn-line">It is called Liberty Leading the People.</span> <span class="yarn-meta">#line:louvre_liberty_2</span>
<span class="yarn-line">by the French artist Eugène Delacroix.</span> <span class="yarn-meta">#line:louvre_liberty_3</span>

</code>
</pre>
</div>

<a id="ys-node-louvre-venus"></a>

## louvre_venus

<div class="yarn-node" data-title="louvre_venus">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=WOMAN, </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset venusmilo&gt;&gt;</span>
<span class="yarn-line">This is the Venus de Milo, an ancient Greek statue.</span> <span class="yarn-meta">#line:053d4fe </span>

</code>
</pre>
</div>

<a id="ys-node-npc-louvre-pyramid"></a>

## npc_louvre_pyramid

<div class="yarn-node" data-title="npc_louvre_pyramid">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: louvre</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card louvre_pyramid&gt;&gt;</span>
<span class="yarn-line">This glass pyramid is the main entrance of the museum.</span> <span class="yarn-meta">#line:fr01_pyramid_1</span>
<span class="yarn-line">It was built in the 1980s to welcome more visitors.</span> <span class="yarn-meta">#line:fr01_pyramid_2</span>

</code>
</pre>
</div>

<a id="ys-node-npc-code-of-hammurabi"></a>

## npc_code_of_hammurabi

<div class="yarn-node" data-title="npc_code_of_hammurabi">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: louvre</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card code_of_hammurabi&gt;&gt;</span>
<span class="yarn-line">This stone has very old laws from ancient Mesopotamia.</span> <span class="yarn-meta">#line:fr01_hammurabi_1</span>
<span class="yarn-line">They were written almost 4,000 years ago.</span> <span class="yarn-meta">#line:fr01_hammurabi_2</span>

</code>
</pre>
</div>

<a id="ys-node-npc-coronation-of-napoleon-david"></a>

## npc_coronation_of_napoleon_david

<div class="yarn-node" data-title="npc_coronation_of_napoleon_david">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: louvre</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card coronation_of_napoleon_david&gt;&gt;</span>
<span class="yarn-line">This big painting shows Napoleon becoming emperor.</span> <span class="yarn-meta">#line:fr01_coronation_1</span>
<span class="yarn-line">The artist Jacques-Louis David painted many details.</span> <span class="yarn-meta">#line:fr01_coronation_2</span>

</code>
</pre>
</div>

<a id="ys-node-npc-oath-of-the-horatii-david"></a>

## npc_oath_of_the_horatii_david

<div class="yarn-node" data-title="npc_oath_of_the_horatii_david">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: louvre</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card oath_of_the_horatii_david&gt;&gt;</span>
<span class="yarn-line">This painting shows brothers making a brave promise.</span> <span class="yarn-meta">#line:fr01_horatii_1</span>
<span class="yarn-line">It teaches duty and courage from ancient Rome.</span> <span class="yarn-meta">#line:fr01_horatii_2</span>

</code>
</pre>
</div>

<a id="ys-node-npc-the-seated-scribe"></a>

## npc_the_seated_scribe

<div class="yarn-node" data-title="npc_the_seated_scribe">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: louvre</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card the_seated_scribe&gt;&gt;</span>
<span class="yarn-line">This statue shows a man writing in ancient Egypt.</span> <span class="yarn-meta">#line:fr01_scribe_1</span>
<span class="yarn-line">His eyes look very real and bright.</span> <span class="yarn-meta">#line:fr01_scribe_2</span>

</code>
</pre>
</div>

<a id="ys-node-npc-winged-victory-of-samothrace"></a>

## npc_winged_victory_of_samothrace

<div class="yarn-node" data-title="npc_winged_victory_of_samothrace">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: louvre</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card winged_victory_of_samothrace&gt;&gt;</span>
<span class="yarn-line">This statue shows a winged figure landing on a ship.</span> <span class="yarn-meta">#line:fr01_victory_1</span>
<span class="yarn-line">The wind shapes its clothes and wings.</span> <span class="yarn-meta">#line:fr01_victory_2</span>

</code>
</pre>
</div>

<a id="ys-node-talk-cook"></a>

## talk_cook

<div class="yarn-node" data-title="talk_cook">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">actor: CRAZY_MAN</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $COLLECTED_ITEMS &gt;= 4&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Help me! Antura made a mess in my kitchen!</span> <span class="yarn-meta">#line:07bbb10 </span>
<span class="yarn-line">    I cannot find the ingredients to make the baguette.</span> <span class="yarn-meta">#line:09e867c </span>
    <span class="yarn-cmd">&lt;&lt;asset  baguette&gt;&gt;</span>
<span class="yarn-line">    Our French bread!</span> <span class="yarn-meta">#line:0874503 </span>
    <span class="yarn-cmd">&lt;&lt;set $QUEST_ITEMS = 4&gt;&gt;</span>
<span class="yarn-line">    Please bring me four things:</span> <span class="yarn-meta">#line:07d64c7 </span>
<span class="yarn-line">    flour, water, yeast, and salt.</span> <span class="yarn-meta">#line:0c01530 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-visit-louvre"></a>

## visit_louvre

<div class="yarn-node" data-title="visit_louvre">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset louvre_inside&gt;&gt;</span>
<span class="yarn-line">There are many statues and paintings here.</span> <span class="yarn-meta">#line:08dc97f </span>
<span class="yarn-cmd">&lt;&lt;jump find_monalisa&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-find-monalisa"></a>

## find_monalisa

<div class="yarn-node" data-title="find_monalisa">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>

<span class="yarn-cmd">&lt;&lt;action monalisa&gt;&gt;</span>
<span class="yarn-line">Go find the Mona Lisa!</span> <span class="yarn-meta">#line:0442392 </span>

</code>
</pre>
</div>

<a id="ys-node-go-bakery"></a>

## go_bakery

<div class="yarn-node" data-title="go_bakery">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">tags: actor=WOMAN_OLD</span>
<span class="yarn-header-dim">---</span>
 
<span class="yarn-line">Now look for Antura! It went to the bakery for a baguette!</span> <span class="yarn-meta">#line:076ef0f </span>
<span class="yarn-line">Hurry up!</span> <span class="yarn-meta">#line:0e9c3e7 </span>

</code>
</pre>
</div>

<a id="ys-node-baguette-salt"></a>

## baguette_salt

<div class="yarn-node" data-title="baguette_salt">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=MAN_BIG</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card baguette_salt&gt;&gt;</span>
<span class="yarn-line">This is salt.</span> <span class="yarn-meta">#line:00f1d2f </span>

</code>
</pre>
</div>

<a id="ys-node-baguette-flour"></a>

## baguette_flour

<div class="yarn-node" data-title="baguette_flour">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=MAN_BIG</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card baguette_flour&gt;&gt;</span>
<span class="yarn-line">This is flour.</span> <span class="yarn-meta">#line:06022b0 </span>

</code>
</pre>
</div>

<a id="ys-node-baguette-water"></a>

## baguette_water

<div class="yarn-node" data-title="baguette_water">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=MAN_BIG</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card baguette_water&gt;&gt;</span>
<span class="yarn-line">This is water.</span> <span class="yarn-meta">#line:0c4d1f6 </span>

</code>
</pre>
</div>

<a id="ys-node-baguette-yeast"></a>

## baguette_yeast

<div class="yarn-node" data-title="baguette_yeast">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=MAN_BIG</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card baguette_yeast&gt;&gt;</span>
<span class="yarn-line">This is yeast.</span> <span class="yarn-meta">#line:025865d </span>

</code>
</pre>
</div>

<a id="ys-node-npc-french-guide"></a>

## npc_french_guide

<div class="yarn-node" data-title="npc_french_guide">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: generic</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hello. What do you want to know?</span> <span class="yarn-meta">#line:0070084 </span>
<span class="yarn-line">What is the Eiffel Tower?</span> <span class="yarn-meta">#line:0d91dc0 </span>
<span class="yarn-line">    A tall iron tower, 300 meters high.</span> <span class="yarn-meta">#line:0f17af0 </span>
<span class="yarn-line">    It is a symbol of Paris!</span> <span class="yarn-meta">#line:07a113f </span>
<span class="yarn-line">Where are we?</span> <span class="yarn-meta">#line:09dd1da </span>
<span class="yarn-line">    We are in Paris.</span> <span class="yarn-meta">#line:02b627d </span>
<span class="yarn-line">Is this place real?</span> <span class="yarn-meta">#line:08bede4 </span>
<span class="yarn-line">    Yes! Why do you ask?</span> <span class="yarn-meta">#line:08654e6 </span>
<span class="yarn-line">    Well... it looks like a video game.</span> <span class="yarn-meta">#line:0bc62a3 </span>
<span class="yarn-line">Nothing. Bye.</span> <span class="yarn-meta">#line:0fe0732 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-eiffell-tourist"></a>

## spawned_eiffell_tourist

<div class="yarn-node" data-title="spawned_eiffell_tourist">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: eiffel_tower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I would like to go up the Eiffel Tower.</span> <span class="yarn-meta">#line:0aee9bb </span>
<span class="yarn-line">You need a ticket to go up.</span> <span class="yarn-meta">#line:09be864 </span>
<span class="yarn-line">There was a big fair in Paris in 1889.</span> <span class="yarn-meta">#line:0a3f4e1 </span>
<span class="yarn-line">    It was to celebrate the 100th anniversary of the French Revolution.</span> <span class="yarn-meta">#line:01fa210 </span>
<span class="yarn-line">    The Eiffel Tower was built for that event.</span> <span class="yarn-meta">#line:0d6f3c4 </span>
<span class="yarn-line">I love Paris!</span> <span class="yarn-meta">#line:0bda18a </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-man"></a>

## spawned_man

<div class="yarn-node" data-title="spawned_man">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: generic</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Do you have questions?</span> <span class="yarn-meta">#line:07b94e9 </span>
<span class="yarn-line">Have you seen Antura?</span> <span class="yarn-meta">#line:0f18ad3 </span>
<span class="yarn-line">    Yes! Talk to people and follow the lights!</span> <span class="yarn-meta">#line:0cf9b4e </span>
<span class="yarn-line">    No. Who is Antura?</span> <span class="yarn-meta">#line:0f9dd62 </span>
<span class="yarn-line">What are you doing?</span> <span class="yarn-meta">#line:002796f </span>
<span class="yarn-line">    I am going to work!</span> <span class="yarn-meta">#line:0fe4ff4 </span>
<span class="yarn-line">    I am going to buy bread at the bakery.</span> <span class="yarn-meta">#line:05a38a8 </span>
<span class="yarn-line">Where do you come from?</span> <span class="yarn-meta">#line:05eabcf </span>
<span class="yarn-line">    I was not born in this country.</span> <span class="yarn-meta">#line:0635a6a </span>
<span class="yarn-line">    From planet Earth.</span> <span class="yarn-meta">#line:0749690 </span>
<span class="yarn-line">Goodbye</span> <span class="yarn-meta">#line:0ee51fc </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kid-m"></a>

## spawned_kid_m

<div class="yarn-node" data-title="spawned_kid_m">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">tags: actor=KID_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: kids</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hi!</span> <span class="yarn-meta">#line:0c4d9e4 </span>
<span class="yarn-line">How are you?</span> <span class="yarn-meta">#line:032d401 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kid-f"></a>

## spawned_kid_f

<div class="yarn-node" data-title="spawned_kid_f">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">tags: actor=KID_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: kids</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour!</span> <span class="yarn-meta">#line:041403d </span>
<span class="yarn-line">Ca va?</span> <span class="yarn-meta">#line:04986a3 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-louvre-museum"></a>

## npc_louvre_museum

<div class="yarn-node" data-title="npc_louvre_museum">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: louvre</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The Louvre is one of the largest museums in the world.</span> <span class="yarn-meta">#line:fr01_louvre_rand_1</span>
<span class="yarn-line">You can walk here for hours and still not see everything.</span> <span class="yarn-meta">#line:fr01_louvre_rand_2</span>
<span class="yarn-line">Many artworks here are older than your grandparents' grandparents.</span> <span class="yarn-meta">#line:fr01_louvre_rand_3</span>
<span class="yarn-line">The glass pyramid lets in light for the halls below.</span> <span class="yarn-meta">#line:fr01_louvre_rand_4</span>

</code>
</pre>
</div>

<a id="ys-node-npc-notredame-base"></a>

## npc_notredame_base

<div class="yarn-node" data-title="npc_notredame_base">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: notredame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Notre-Dame is a famous Gothic cathedral.</span> <span class="yarn-meta">#line:fr01_notredame_base_1</span>
<span class="yarn-line">Builders started it more than 800 years ago.</span> <span class="yarn-meta">#line:fr01_notredame_base_2</span>
<span class="yarn-line">The big bells ring across the city.</span> <span class="yarn-meta">#line:fr01_notredame_base_3</span>

</code>
</pre>
</div>

<a id="ys-node-npc-notredame-roof"></a>

## npc_notredame_roof

<div class="yarn-node" data-title="npc_notredame_roof">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: notredame_roof</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">From the roof you can see much of Paris.</span> <span class="yarn-meta">#line:fr01_notredame_roof_1</span>
<span class="yarn-line">Stone creatures called gargoyles sit up here.</span> <span class="yarn-meta">#line:fr01_notredame_roof_2</span>
<span class="yarn-line">Workers are still restoring parts of the cathedral.</span> <span class="yarn-meta">#line:fr01_notredame_roof_3</span>

</code>
</pre>
</div>

<a id="ys-node-npc-bakery"></a>

## npc_bakery

<div class="yarn-node" data-title="npc_bakery">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">tags: actor=MAN_BIG</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: bakery</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Fresh bread smell makes people happy.</span> <span class="yarn-meta">#line:fr01_bakery_1</span>
<span class="yarn-line">A baguette uses flour, water, yeast, and salt.</span> <span class="yarn-meta">#line:fr01_bakery_2</span>
<span class="yarn-line">Bakers wake up very early to start making dough.</span> <span class="yarn-meta">#line:fr01_bakery_3</span>

</code>
</pre>
</div>


