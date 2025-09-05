---
title: Parigi! (fr_01) - Script
hide:
---

# Parigi! (fr_01) - Script
[Quest Index](./index.it.md) - Language: [english](./fr_01-script.md) - [french](./fr_01-script.fr.md) - [polish](./fr_01-script.pl.md) - italian

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/fr-01-paris/23/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=755037318#gid=755037318)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_01%20Paris/FR_01%20Paris%20-%20Yarn%20Script.yarn)  

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// FR_01 PARIS</span>
<span class="yarn-header-dim">// Cards:</span>
<span class="yarn-header-dim">// - tour_eiffell (landmark education)</span>
<span class="yarn-header-dim">// - mr_eiffel (historical figure)</span>
<span class="yarn-header-dim">// - tour_eiffell_ticket (cultural practice)</span>
<span class="yarn-header-dim">// - tour_eiffell_map (navigation aid)</span>
<span class="yarn-header-dim">// - louvre (landmark education)</span>
<span class="yarn-header-dim">// - louvre_inside (cultural immersion)</span>
<span class="yarn-header-dim">// - monalisa (art education)</span>
<span class="yarn-header-dim">// - leaonardodavinci (historical figure)</span>
<span class="yarn-header-dim">// - liberty_leading (art education)</span>
<span class="yarn-header-dim">// - venusmilo (art education)</span>
<span class="yarn-header-dim">// - notredame (landmark education)</span>
<span class="yarn-header-dim">// - notredame_fire (recent history)</span>
<span class="yarn-header-dim">// - baguette (cultural food)</span>
<span class="yarn-header-dim">// - baguette_flour (ingredient education)</span>
<span class="yarn-header-dim">// - baguette_water (ingredient education)</span>
<span class="yarn-header-dim">// - baguette_yeast (ingredient education)</span>
<span class="yarn-header-dim">// - baguette_salt (ingredient education)</span>
<span class="yarn-header-dim">// Tasks:</span>
<span class="yarn-header-dim">// - Collect 4 baguette ingredients from various locations</span>
<span class="yarn-header-dim">// - Reach Monalisa</span>
<span class="yarn-header-dim">// Activities:</span>
<span class="yarn-header-dim">// - puzzle - Eiffel Tower</span>
<span class="yarn-header-dim">// Words: </span>
<span class="yarn-header-dim">// ticket, coin, baguette, flour, water, yeast, salt, map, elevator, iron, museum, cathedral, painting, sculpture,</span>
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

<span class="yarn-line">Welcome to Paris! <span class="yarn-meta">#line:fr01_start </span></span>
<span class="yarn-line">Go and talk with the tutor! <span class="yarn-meta">#line:fr01_start_2</span></span>
<span class="yarn-cmd">&lt;&lt;target tutor&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-talk-tutor"></a>
## talk_tutor

<div class="yarn-node" data-title="talk_tutor"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">actor: tutor</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I saw Antura going to the Eiffel Tower. <span class="yarn-meta">#line:fr01_talk_tutor</span></span>
<span class="yarn-cmd">&lt;&lt;camera_focus tour_eiffell&gt;&gt;</span>
<span class="yarn-line">Follow the target or use the map! <span class="yarn-meta">#line:fr01_talk_tutor_2 </span></span>
<span class="yarn-line">Get there as soon as possible! <span class="yarn-meta">#line:fr01_talk_tutor_3 </span></span>

</code></pre></div>

<a id="ys-node-talk-louvre-external"></a>
## talk_louvre_external

<div class="yarn-node" data-title="talk_louvre_external"><pre class="yarn-code"><code><span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">actor: OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $MET_MONALISA&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump go_bakery&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;asset louvre&gt;&gt;</span>
<span class="yarn-line">This is the entrance to the Louvre, our national art museum. <span class="yarn-meta">#line:0cf1cc8 </span></span>
<span class="yarn-line">Do you want to enter? <span class="yarn-meta">#line:0f74ff9</span></span>
<span class="yarn-line">-&gt; Yes: <span class="yarn-meta">#line:090114f </span></span>
<span class="yarn-line">    Enjoy your visit! <span class="yarn-meta">#line:056e051 </span></span>
    <span class="yarn-cmd">&lt;&lt;action AREA_LOUVRE_ENTER &gt;&gt;</span>
<span class="yarn-line">-&gt; No: <span class="yarn-meta">#line:077422a </span></span>
<span class="yarn-line">    Okay. <span class="yarn-meta">#line:0c28ea0 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-talk-eiffell-roof"></a>
## talk_eiffell_roof

<div class="yarn-node" data-title="talk_eiffell_roof"><pre class="yarn-code"><code><span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset toureiffell&gt;&gt;</span>
<span class="yarn-line">The Eiffel Tower is 300 meters tall. <span class="yarn-meta">#line:08c1973 </span></span>
<span class="yarn-cmd">&lt;&lt;asset mr_eiffel&gt;&gt;</span>
<span class="yarn-line">Built by Mr Eiffel in 1887. <span class="yarn-meta">#line:09e5c3b </span></span>
<span class="yarn-cmd">&lt;&lt;asset iron&gt;&gt;</span>
<span class="yarn-line">It's made of iron! <span class="yarn-meta">#line:0d59ade </span></span>
<span class="yarn-line">I saw Antura going towards Notre Dame. <span class="yarn-meta">#line:04d1e52 </span></span>
<span class="yarn-cmd">&lt;&lt;camera_focus notredame&gt;&gt;</span>
<span class="yarn-line">Get there! <span class="yarn-meta">#line:083b3bf </span></span>


</code></pre></div>

<a id="ys-node-talk-eiffell-guide"></a>
## talk_eiffell_guide

<div class="yarn-node" data-title="talk_eiffell_guide"><pre class="yarn-code"><code><span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">actor: guide</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $TOTAL_COINS &gt; 2&gt;&gt;
<span class="yarn-line">    Here is your ticket. <span class="yarn-meta">#line:04e74ad </span></span>
    <span class="yarn-cmd">&lt;&lt;asset tour_eiffell_ticket&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = $TOTAL_COINS-3&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action area_toureiffel&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset tour_eiffell_map&gt;&gt;</span>
<span class="yarn-line">    I saw Antura going up to the top of the tower. <span class="yarn-meta">#line:089abda </span></span>
<span class="yarn-line">    Take the elevator! <span class="yarn-meta">#line:0585a5e </span></span>
&lt;&lt;elseif $TOTAL_COINS &gt; 0&gt;&gt; 
<span class="yarn-line">    Collect all coins! <span class="yarn-meta">#line:04d966b </span></span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    The ticket for the Eiffel Tower costs 3 coins. <span class="yarn-meta">#line:069cbb3 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-talk-notre-dame"></a>
## talk_notre_dame

<div class="yarn-node" data-title="talk_notre_dame"><pre class="yarn-code"><code><span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">tags: actor=OLD_MAN, asset=notredame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I'm the Mayor of Paris. <span class="yarn-meta">#line:0cc11fa </span></span>
<span class="yarn-line">This is Notre-Dame Cathedral. <span class="yarn-meta">#line:06f3fa2 </span></span>
<span class="yarn-cmd">&lt;&lt;asset notredame&gt;&gt;</span>
<span class="yarn-line">It is a famous Gothic church, built in 1182. <span class="yarn-meta">#line:02edc0f </span></span>
<span class="yarn-cmd">&lt;&lt;action AREA_NOTREDAME_ROOF&gt;&gt;</span>
<span class="yarn-line">Come with me up to the roof of the church! <span class="yarn-meta">#line:083dfcc </span></span>
<span class="yarn-cmd">&lt;&lt;set $MET_MAJOR = true&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-talk-cook"></a>
## talk_cook

<div class="yarn-node" data-title="talk_cook"><pre class="yarn-code"><code><span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">actor: CRAZY_MAN</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $COLLECTED_ITEMS &gt;= 4&gt;&gt;
<span class="yarn-line">    GREAT! I can now bake the Baguette. And... <span class="yarn-meta">#line:0017917 </span></span>
<span class="yarn-line">    CONGRATULATIONS! You won the game! Did you like it? <span class="yarn-meta">#line:0d11596 </span></span>
    <span class="yarn-cmd">&lt;&lt;quest_end 3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Help me! Antura made a mess in my kitchen! <span class="yarn-meta">#line:07bbb10 </span></span>
<span class="yarn-line">    I can't find the ingredients to make the baguette. <span class="yarn-meta">#line:09e867c </span></span>
    <span class="yarn-cmd">&lt;&lt;asset  baguette&gt;&gt;</span>
<span class="yarn-line">    Our special French bread! <span class="yarn-meta">#line:0874503 </span></span>
    <span class="yarn-cmd">&lt;&lt;set $QUEST_ITEMS = 4&gt;&gt;</span>
<span class="yarn-line">    Please bring me 4 ingredients: <span class="yarn-meta">#line:07d64c7 </span></span>
<span class="yarn-line">    flour, water, yeast, and salt. <span class="yarn-meta">#line:0c01530 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-visit-louvre"></a>
## visit_louvre

<div class="yarn-node" data-title="visit_louvre"><pre class="yarn-code"><code><span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">tags: actor=CRAZY_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset louvre_inside&gt;&gt;</span>
<span class="yarn-line">You can find many sculptures and paintings. <span class="yarn-meta">#line:08dc97f </span></span>
<span class="yarn-cmd">&lt;&lt;jump find_monalisa&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-find-monalisa"></a>
## find_monalisa

<div class="yarn-node" data-title="find_monalisa"><pre class="yarn-code"><code><span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">tags: actor=CRAZY_WOMAN</span>
<span class="yarn-header-dim">---</span>

<span class="yarn-cmd">&lt;&lt;action monalisa&gt;&gt;</span>
<span class="yarn-line">Go and find the Mona Lisa! <span class="yarn-meta">#line:0442392 </span></span>

</code></pre></div>

<a id="ys-node-go-bakery"></a>
## go_bakery

<div class="yarn-node" data-title="go_bakery"><pre class="yarn-code"><code><span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
 
<span class="yarn-line">Now look for Antura! It went to the bakery for a baguette! <span class="yarn-meta">#line:076ef0f </span></span>
<span class="yarn-line">Hurry up! <span class="yarn-meta">#line:0e9c3e7 </span></span>

</code></pre></div>

<a id="ys-node-baguette-salt"></a>
## baguette_salt

<div class="yarn-node" data-title="baguette_salt"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=CRAZY_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;asset baguette_salt&gt;&gt;</span>
<span class="yarn-line">This is salt. <span class="yarn-meta">#line:00f1d2f </span></span>

</code></pre></div>

<a id="ys-node-baguette-flour"></a>
## baguette_flour

<div class="yarn-node" data-title="baguette_flour"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=CRAZY_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset baguette_flour&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_2&gt;&gt;</span>
<span class="yarn-line">This is flour. <span class="yarn-meta">#line:06022b0 </span></span>

</code></pre></div>

<a id="ys-node-baguette-water"></a>
## baguette_water

<div class="yarn-node" data-title="baguette_water"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=CRAZY_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;asset baguette_water&gt;&gt;</span>
<span class="yarn-line">This is water. <span class="yarn-meta">#line:0c4d1f6 </span></span>

</code></pre></div>

<a id="ys-node-baguette-yeast"></a>
## baguette_yeast

<div class="yarn-node" data-title="baguette_yeast"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=CRAZY_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;asset baguette_yeast&gt;&gt;</span>
<span class="yarn-line">This is yeast. <span class="yarn-meta">#line:025865d </span></span>

</code></pre></div>

<a id="ys-node-talk-notre-dame-roof"></a>
## talk_notre_dame_roof

<div class="yarn-node" data-title="talk_notre_dame_roof"><pre class="yarn-code"><code><span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">tags: actor=OLD_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset notredame_fire&gt;&gt;</span>
<span class="yarn-line">There was a big fire in 2019, but we could repair it. <span class="yarn-meta">#line:09a0ead </span></span>
<span class="yarn-line">I saw Antura run into the Louvre Museum. <span class="yarn-meta">#line:02ba888 </span></span>
<span class="yarn-line">It is just across the River Seine. <span class="yarn-meta">#line:00d22e5 </span></span>

</code></pre></div>

<a id="ys-node-talk-louvre-guide"></a>
## talk_louvre_guide

<div class="yarn-node" data-title="talk_louvre_guide"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">tags: actor=CRAZY_WOMAN</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Welcome to the Louvre Museum. What do you want to do? <span class="yarn-meta">#line:0e6d2a5 </span></span>
<span class="yarn-line">-&gt; Tell me about the Louvre <span class="yarn-meta">#line:0a5fc63 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump visit_louvre&gt;&gt;</span>
<span class="yarn-line">-&gt; Exit <span class="yarn-meta">#line:0efc18f </span></span>
    <span class="yarn-cmd">&lt;&lt;if $MET_MONALISA&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action AREA_LOUVRE_EXIT&gt;&gt;</span>
<span class="yarn-line">        Come back! <span class="yarn-meta">#line:07dd921 </span></span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump find_monalisa&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-louvre-monalisa"></a>
## louvre_monalisa

<div class="yarn-node" data-title="louvre_monalisa"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=CRAZY_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset monalisa&gt;&gt;</span>
<span class="yarn-line">This is the famous Mona Lisa. <span class="yarn-meta">#line:louvre_monalisa_1</span></span>
<span class="yarn-cmd">&lt;&lt;set $MET_MONALISA = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;asset leaonardodavinci&gt;&gt;</span>
<span class="yarn-line">It was painted around 1500 <span class="yarn-meta">#line:louvre_monalisa_2</span></span>
<span class="yarn-line">by the artist and scientist Leonardo da Vinci. <span class="yarn-meta">#line:louvre_monalisa_3</span></span>

</code></pre></div>

<a id="ys-node-louvre-liberty"></a>
## louvre_liberty

<div class="yarn-node" data-title="louvre_liberty"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=CRAZY_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset liberty_leading&gt;&gt;</span>
<span class="yarn-line">This painting represents freedom. <span class="yarn-meta">#line:louvre_liberty_1</span></span>
<span class="yarn-line">It's called Liberty Leading the People <span class="yarn-meta">#line:louvre_liberty_2</span></span>
<span class="yarn-line">by the French artist Eugène Delacroix <span class="yarn-meta">#line:louvre_liberty_3</span></span>

</code></pre></div>

<a id="ys-node-louvre-venus"></a>
## louvre_venus

<div class="yarn-node" data-title="louvre_venus"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=CRAZY_WOMAN, </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset venusmilo&gt;&gt;</span>
<span class="yarn-line">The Venus de Milo, an ancient Greek marble sculpture. <span class="yarn-meta">#line:053d4fe </span></span>

</code></pre></div>

<a id="ys-node-npc-woman"></a>
## npc_woman

<div class="yarn-node" data-title="npc_woman"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ciao. What do you want to know? <span class="yarn-meta">#line:0070084 </span></span>
<span class="yarn-line">-&gt; What is the Eiffel Tower? <span class="yarn-meta">#line:0d91dc0 </span></span>
<span class="yarn-line">    The famous iron tower, 300 meters tall. <span class="yarn-meta">#line:0f17af0 </span></span>
<span class="yarn-line">    The symbol of Paris! <span class="yarn-meta">#line:07a113f </span></span>
<span class="yarn-line">-&gt; Where are we? <span class="yarn-meta">#line:09dd1da </span></span>
<span class="yarn-line">    We are in Paris, the city of love! <span class="yarn-meta">#line:02b627d </span></span>
<span class="yarn-line">-&gt; Is this place real? <span class="yarn-meta">#line:08bede4 </span></span>
<span class="yarn-line">    Sure! Why are you asking? <span class="yarn-meta">#line:08654e6 </span></span>
<span class="yarn-line">    Well... it looks like a video game, doesn't it? <span class="yarn-meta">#line:0bc62a3 </span></span>
<span class="yarn-line">-&gt; Nothing. Bye. <span class="yarn-meta">#line:0fe0732 </span></span>

</code></pre></div>

<a id="ys-node-npc-man"></a>
## npc_man

<div class="yarn-node" data-title="npc_man"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Do you have any questions? <span class="yarn-meta">#line:07b94e9 </span></span>
<span class="yarn-line">-&gt; Have you seen Antura? <span class="yarn-meta">#line:0f18ad3 </span></span>
<span class="yarn-line">    =&gt; Yes! Talk to everybody and follow the lights! <span class="yarn-meta">#line:0cf9b4e </span></span>
<span class="yarn-line">    =&gt; No. Who is Antura? <span class="yarn-meta">#line:0f9dd62 </span></span>
<span class="yarn-line">-&gt; What are you doing? <span class="yarn-meta">#line:002796f </span></span>
<span class="yarn-line">    =&gt; I'm going to work! <span class="yarn-meta">#line:0fe4ff4 </span></span>
<span class="yarn-line">    =&gt; I'm going to buy bread at the bakery. <span class="yarn-meta">#line:05a38a8 </span></span>
<span class="yarn-line">-&gt; Where do you come from? <span class="yarn-meta">#line:05eabcf </span></span>
<span class="yarn-line">    =&gt; I wasn't born in this country. <span class="yarn-meta">#line:0635a6a </span></span>
<span class="yarn-line">    =&gt; From planet Earth. <span class="yarn-meta">#line:0749690 </span></span>
<span class="yarn-line">-&gt; Goodbye <span class="yarn-meta">#line:0ee51fc </span></span>

</code></pre></div>

<a id="ys-node-npc-kid"></a>
## npc_kid

<div class="yarn-node" data-title="npc_kid"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=KID_MALE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">=&gt; Hi! <span class="yarn-meta">#line:0c4d9e4 </span></span>
<span class="yarn-line">=&gt; How are you? <span class="yarn-meta">#line:032d401 </span></span>

</code></pre></div>


