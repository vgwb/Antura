---
title: Opowieść o dwóch zamkach (fr_05) - Script
hide:
---

# Opowieść o dwóch zamkach (fr_05) - Script
[Quest Index](./index.pl.md) - Language: [english](./fr_05-script.md) - [french](./fr_05-script.fr.md) - polish - [italian](./fr_05-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/fr-05-a-tale-of-two-castles/26/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1463729744#gid=1463729744)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_05%20Loire%20Castles/FR_05%20Loire%20Castles%20-%20Yarn%20Script.yarn)  

<div class="yarn-node"><pre class="yarn-code"><code><span class="yarn-header-dim">// Quest: fr_05 | Castles (Loire)</span>
<span class="yarn-header-dim">// FR_05 CASTLES - A tale of two castles</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">// Cards:</span>
<span class="yarn-header-dim">// - chinon (historical castle)</span>
<span class="yarn-header-dim">// - chambord (renaissance castle)</span>
<span class="yarn-header-dim">// - chinon_defence (military architecture)</span>
<span class="yarn-header-dim">// - chambord_ball (royal culture)</span>
<span class="yarn-header-dim">// - bridge (castle architecture)</span>
<span class="yarn-header-dim">// - parapet (defensive architecture)</span>
<span class="yarn-header-dim">// - loopholes (military feature)</span>
<span class="yarn-header-dim">// - obj_helmet (knight equipment)</span>
<span class="yarn-header-dim">// - obj_sword (knight equipment)</span>
<span class="yarn-header-dim">// - obj_bow (knight equipment)</span>
<span class="yarn-header-dim">// - obj_armor (knight equipment)</span>
<span class="yarn-header-dim">// - obj_hat (noble attire)</span>
<span class="yarn-header-dim">// - obj_musical_instruments (court culture)</span>
<span class="yarn-header-dim">// - obj_dance_shoes (court culture)</span>
<span class="yarn-header-dim">// - obj_ball_mask (court culture)</span>
<span class="yarn-header-dim">// - map (regional geography)</span>
<span class="yarn-header-dim">// Tasks:</span>
<span class="yarn-header-dim">// - Collect knight items: helmet, sword, bow, armor</span>
<span class="yarn-header-dim">// - Collect prince items: hat, musical instrument, dance shoes, ball mask</span>
<span class="yarn-header-dim">// - Explore different castle functions (defense vs luxury)</span>
<span class="yarn-header-dim">// Activities:</span>
<span class="yarn-header-dim">// Words used: castle, Chinon, Chambord, drawbridge, parapet, loopholes, defense, knight, armor, sword, bow, helmet, prince, mask, shoes, map, room, interior, Loire Valley, medieval, royal, French heritage</span>
<span class="yarn-header-dim">// </span>
</code></pre></div>

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">=</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = 0&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Welcome to the castles of the Loire!] <span class="yarn-meta">#line:09dda7c </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-the-end"></a>
## the_end

<div class="yarn-node" data-title="the_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: The game is complete! Congratulations!]
[MISSING TRANSLATION: Did you like it?]
<span class="yarn-cmd">&lt;&lt;jump quest_proposal&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-quest-proposal"></a>
## quest_proposal

<div class="yarn-node" data-title="quest_proposal"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: Now draw your favourite castle!]
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-talk-guide"></a>
## talk_guide

<div class="yarn-node" data-title="talk_guide"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $COLLECTED_ITEMS &gt;= 8&gt;&gt;
<span class="yarn-cmd">&lt;&lt;jump guide_question&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Antura ran away and is hiding in a castle!] <span class="yarn-meta">#line:067c028 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-guide-defence"></a>
## guide_defence

<div class="yarn-node" data-title="guide_defence"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN, asset=chinon</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: This is the castle of Chinon.] <span class="yarn-meta">#line:06eaf5c </span></span>
<span class="yarn-cmd">&lt;&lt;jump FR_05_CASTLES_Text_98_0a_8d&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-guide-living"></a>
## guide_living

<div class="yarn-node" data-title="guide_living"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN, asset=chambord</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Welcome to Chambord Castle.] <span class="yarn-meta">#line:0a06d3f </span></span>
<span class="yarn-cmd">&lt;&lt;jump FR_05_CASTLES_Text_91139071&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-fr-05-castles-text-02040108"></a>
## FR_05_CASTLES_Text_02040108

<div class="yarn-node" data-title="FR_05_CASTLES_Text_02040108"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Find all the things a knight wears:] <span class="yarn-meta">#line:0bfad5e </span></span>
<span class="yarn-line">[MISSING TRANSLATION: helmet, sword, bow, and armor.] <span class="yarn-meta">#line:08b44fc </span></span>
 [MISSING TRANSLATION: ]
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-fr-05-castles-text-02082173"></a>
## FR_05_CASTLES_Text_02082173

<div class="yarn-node" data-title="FR_05_CASTLES_Text_02082173"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Find all the prince's items:] <span class="yarn-meta">#line:0cb22cb </span></span>
<span class="yarn-line">[MISSING TRANSLATION: hat, musical instrument, dance shoes, and ball mask.] <span class="yarn-meta">#line:0d2be40 </span></span>
 [MISSING TRANSLATION: ]
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-guide-question"></a>
## guide_question

<div class="yarn-node" data-title="guide_question"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Did you find Antura?] <span class="yarn-meta">#line:01003d0 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; "YES":] <span class="yarn-meta">#line:0d9f509 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump guide_done&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: -&gt; "NO":] <span class="yarn-meta">#line:0de6ebb </span></span>
    <span class="yarn-cmd">&lt;&lt;jump guide_done&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-guide-done"></a>
## guide_done

<div class="yarn-node" data-title="guide_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset map&gt;&gt;</span>
[MISSING TRANSLATION: Did you like the castles?]
<span class="yarn-line">[MISSING TRANSLATION: There are 200 castles like this in the Loire Valley!] <span class="yarn-meta">#line:01e6614 </span></span>
<span class="yarn-cmd">&lt;&lt;jump the_end&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-view-bridge"></a>
## view_bridge

<div class="yarn-node" data-title="view_bridge"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN, asset=bridge</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: A drawbridge is a kind of movable bridge used in castles.] <span class="yarn-meta">#line:0010896 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-view-parapet"></a>
## view_parapet

<div class="yarn-node" data-title="view_parapet"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN, asset=parapet</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: A parapet has gaps used for defense.] <span class="yarn-meta">#line:0aaf4dd </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-obg-helmet"></a>
## obg_helmet

<div class="yarn-node" data-title="obg_helmet"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card obj_helmet&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: a helmet] <span class="yarn-meta">#line:0b492f6 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-obj-hat"></a>
## obj_hat

<div class="yarn-node" data-title="obj_hat"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=obj_hat</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: a hat] <span class="yarn-meta">#line:0806c77 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-obg-sword"></a>
## obg_sword

<div class="yarn-node" data-title="obg_sword"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset obj_sword&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: sword] <span class="yarn-meta">#line:0733b81 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-obg-bow"></a>
## obg_bow

<div class="yarn-node" data-title="obg_bow"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset  obj_bow&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: a bow and arrows] <span class="yarn-meta">#line:049d6e8 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-obg-armor"></a>
## obg_armor

<div class="yarn-node" data-title="obg_armor"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset  obj_armor&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: this is an armor] <span class="yarn-meta">#line:01ac543 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-view-loopholes"></a>
## view_loopholes

<div class="yarn-node" data-title="view_loopholes"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset loopholes&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: These are loopholes. They are used to shoot arrows at the enemy.] <span class="yarn-meta">#line:050e177 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-fr-05-castles-text-08-89-98"></a>
## FR_05_CASTLES_Text_08_89_98

<div class="yarn-node" data-title="FR_05_CASTLES_Text_08_89_98"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset  chinon_defence&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: This castle was used for defense.] <span class="yarn-meta">#line:026073b </span></span>
<span class="yarn-cmd">&lt;&lt;jump FR_05_CASTLES_Text_02040108&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-fr-05-castles-text-98-0a-8d"></a>
## FR_05_CASTLES_Text_98_0a_8d

<div class="yarn-node" data-title="FR_05_CASTLES_Text_98_0a_8d"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN, asset=chinon_old</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: It is very old.] <span class="yarn-meta">#line:0a0e0ca </span></span>
<span class="yarn-cmd">&lt;&lt;jump FR_05_CASTLES_Text_08_89_98&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-fr-05-castles-text-91139071"></a>
## FR_05_CASTLES_Text_91139071

<div class="yarn-node" data-title="FR_05_CASTLES_Text_91139071"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN, asset=chambord_ball</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Kings and princes used this castle for balls, plays, and concerts.] <span class="yarn-meta">#line:07d0b04 </span></span>
<span class="yarn-cmd">&lt;&lt;jump FR_05_CASTLES_Text_02082173&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-obj-musical-intrument"></a>
## obj_musical_intrument

<div class="yarn-node" data-title="obj_musical_intrument"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=obj_musical_instruments</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: a musical instrument] <span class="yarn-meta">#line:07f4c19 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-obj-shoes"></a>
## obj_shoes

<div class="yarn-node" data-title="obj_shoes"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=obj_dance_shoes</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: dance shoes] <span class="yarn-meta">#line:01ee90f </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-obj-ball-mask"></a>
## obj_ball_mask

<div class="yarn-node" data-title="obj_ball_mask"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=obj_ball_mask</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: a ball mask] <span class="yarn-meta">#line:0071a08 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-view-chambord-map"></a>
## view_chambord_map

<div class="yarn-node" data-title="view_chambord_map"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=chambord_map</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: This is a map of the castle.] <span class="yarn-meta">#line:0b62127 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-view-chambord-room"></a>
## view_chambord_room

<div class="yarn-node" data-title="view_chambord_room"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=chambord_room</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: What a room!] <span class="yarn-meta">#line:0f99fd5 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-view-interior"></a>
## view_interior

<div class="yarn-node" data-title="view_interior"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=chambord_interior</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: It is very nice!] <span class="yarn-meta">#line:082ac97 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-spawned-tourist"></a>
## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: tourists </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; Did you know that the Loire Valley is a UNESCO World Heritage site?] <span class="yarn-meta">#line:0c3f4e1 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; The castle of Chambord has 440 rooms!] <span class="yarn-meta">#line:0a5f4f1 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; The castle of Chinon was a favorite residence of King Henry II of England.] <span class="yarn-meta">#line:0b6e2c3 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; The Loire Valley is known as the "Garden of France".] <span class="yarn-meta">#line:0d1e4b5 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; The castle of Chambord was built in the 16th century.] <span class="yarn-meta">#line:0e2f6d7 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>


