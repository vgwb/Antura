---
title: The Zoo (pl_04) - Script
hide:
---

# The Zoo (pl_04) - Script
[Quest Index](./index.md) - Language: english - [french](./pl_04-script.fr.md) - [polish](./pl_04-script.pl.md) - [italian](./pl_04-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/pl-04-the-zoo/35/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=819047762#gid=819047762)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_04%20Zoo/PL_04%20Zoo%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>
## quest_start

<div class="yarn-node" data-title="quest_start"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// pl_04 | Zoo (Wroclaw)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: actor=Man, </span>
<span class="yarn-header-dim">image: centennial_hall_empty_flag</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $talked_animals = false&gt;&gt;</span>
<span class="yarn-line">Welcome to WROCŁAW ZOO. <span class="yarn-meta">#line:0fe55d1 </span></span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_zoo&gt;&gt;</span>
<span class="yarn-line">There are many ANIMALS here! <span class="yarn-meta">#line:005dd46 </span></span>

</code></pre></div>

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This quest is complete. <span class="yarn-meta">#line:0bcc257 </span></span>
<span class="yarn-line">You learned about ZOO ANIMALS. <span class="yarn-meta">#line:054cc37 </span></span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Draw your favorite ANIMAL. <span class="yarn-meta">#line:0809ac5 </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-director-talk"></a>
## director_talk

<div class="yarn-node" data-title="director_talk"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: actor=Man, type:Panel</span>
<span class="yarn-header-dim">image: centennial_hall_empty_flag</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $talked_animals == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump director_task_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump director_task&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-director-task"></a>
## director_task

<div class="yarn-node" data-title="director_task"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: actor=Man, type:Panel</span>
<span class="yarn-header-dim">image: centennial_hall_empty_flag</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Oh no! The FLAG is missing! <span class="yarn-meta">#line:09c6bf7 </span></span>
<span class="yarn-cmd">&lt;&lt;camera_focus FlagPole&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card iglica&gt;&gt;</span>
<span class="yarn-line">It was on the IGLICA at CENTENNIAL HALL. <span class="yarn-meta">#line:02f35e4 </span></span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">Please help me find it! <span class="yarn-meta">#line:0fd5d1a </span></span>
<span class="yarn-line">Talk to ANIMALS. One may have it. <span class="yarn-meta">#line:012b933 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_ANIMALS task_animals_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-animals-desc"></a>
## task_animals_desc

<div class="yarn-node" data-title="task_animals_desc"><pre class="yarn-code"><code><span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Find the FLAG! <span class="yarn-meta">#line:0da284c </span></span>
<span class="yarn-line">Talk to all the animals. Maybe they know where the flag is. <span class="yarn-meta">#line:010adc5 </span></span>

</code></pre></div>

<a id="ys-node-animal-peacock"></a>
## animal_peacock

<div class="yarn-node" data-title="animal_peacock"><pre class="yarn-code"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// ENTRANCE – AMBIENCE</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: ZOO</span>
<span class="yarn-header-dim">tags: actor=Peacock</span>
<span class="yarn-header-dim">image: zoo_gate</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I do not need a FLAG. <span class="yarn-meta">#line:0085a8a </span></span>
<span class="yarn-line">My tail is a FLAG! <span class="yarn-meta">#line:04fff6b </span></span>

</code></pre></div>

<a id="ys-node-animal-parrot"></a>
## animal_parrot

<div class="yarn-node" data-title="animal_parrot"><pre class="yarn-code"><code><span class="yarn-header-dim">group: ZOO</span>
<span class="yarn-header-dim">tags: actor=Parrot</span>
<span class="yarn-header-dim">image: zoo_gate</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Squawk! Red and yellow ran by! <span class="yarn-meta">#line:0e84545 </span></span>

</code></pre></div>

<a id="ys-node-elephant-talk"></a>
## elephant_talk

<div class="yarn-node" data-title="elephant_talk"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">// PART 1 – ELEPHANT</span>
<span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: elephant_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I am the biggest land ANIMAL. <span class="yarn-meta">#line:027b51f </span></span>
<span class="yarn-line">I have good MEMORY. <span class="yarn-meta">#line:03a150c </span></span>
<span class="yarn-line">Do you have good MEMORY? <span class="yarn-meta">#line:0f98478 </span></span>
<span class="yarn-cmd">&lt;&lt;activity memory_elephant_settings elephant_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-elephant-activity-done"></a>
## elephant_activity_done

<div class="yarn-node" data-title="elephant_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: elephant_keeper</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;inventory animal_elephant add&gt;&gt;</span>
<span class="yarn-line">A FLAG? I do not have it. <span class="yarn-meta">#line:0b79d01 </span></span>
<span class="yarn-line">If I took it I would remember! <span class="yarn-meta">#line:0f124bf </span></span>

</code></pre></div>

<a id="ys-node-elephant-sign"></a>
## ELEPHANT_SIGN

<div class="yarn-node" data-title="ELEPHANT_SIGN"><pre class="yarn-code"><code><span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">tags: actor=tutor</span>
<span class="yarn-header-dim">image: elephant_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">ELEPHANT. Thick skin. Big EARS. <span class="yarn-meta">#line:048e8a1 </span></span>
<span class="yarn-line">Largest land ANIMAL. <span class="yarn-meta">#line:0b1cca2 </span></span>

</code></pre></div>

<a id="ys-node-elephant-kid"></a>
## ELEPHANT_KID

<div class="yarn-node" data-title="ELEPHANT_KID"><pre class="yarn-code"><code><span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">tags: actor=KID</span>
<span class="yarn-header-dim">image: kid_elephant</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Its EARS are bigger than me! <span class="yarn-meta">#line:0fc78ad </span></span>
<span class="yarn-line">Can it fan me in summer? <span class="yarn-meta">#line:004abc7 </span></span>


</code></pre></div>

<a id="ys-node-giraffe-talk"></a>
## giraffe_talk

<div class="yarn-node" data-title="giraffe_talk"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">// PART 2 – GIRAFFE</span>
<span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: giraffe_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I am the tallest ANIMAL. <span class="yarn-meta">#line:0d5c607 </span></span>
<span class="yarn-line">My long NECK helps me reach leaves. <span class="yarn-meta">#line:0a4d24e </span></span>
<span class="yarn-line">I eat WATTLE leaves! <span class="yarn-meta">#line:04b42f2 </span></span>
<span class="yarn-cmd">&lt;&lt;activity canvas_giraffe_settings giraffe_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-giraffe-activity-done"></a>
## giraffe_activity_done

<div class="yarn-node" data-title="giraffe_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: giraffe_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;inventory animal_elephant add&gt;&gt;</span>
<span class="yarn-line">I did not take the FLAG. <span class="yarn-meta">#line:0877d6f </span></span>
<span class="yarn-line">It is too high for me! <span class="yarn-meta">#line:02d00e2 </span></span>

</code></pre></div>

<a id="ys-node-giraffe-sign"></a>
## GIRAFFE_SIGN

<div class="yarn-node" data-title="GIRAFFE_SIGN"><pre class="yarn-code"><code><span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">tags: actor=Sign</span>
<span class="yarn-header-dim">image: giraffe_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">GIRAFFE. Tall. Long NECK. <span class="yarn-meta">#line:0a8a73f </span></span>
<span class="yarn-line">Long EYELASHES. <span class="yarn-meta">#line:0291317 </span></span>

</code></pre></div>

<a id="ys-node-giraffe-kid"></a>
## GIRAFFE_KID

<div class="yarn-node" data-title="GIRAFFE_KID"><pre class="yarn-code"><code><span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">tags: actor=KID</span>
<span class="yarn-header-dim">image: kid_giraffe</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">With a NECK like that! <span class="yarn-meta">#line:068daeb </span></span>
<span class="yarn-line">I could see my house from here. <span class="yarn-meta">#line:0bee484 </span></span>


</code></pre></div>

<a id="ys-node-lion-talk"></a>
## lion_talk

<div class="yarn-node" data-title="lion_talk"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">// PART 3 – LION</span>
<span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: lion_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I am a LION in AFRICA. <span class="yarn-meta">#line:07f2e15 </span></span>
<span class="yarn-line">I live in a PRIDE. <span class="yarn-meta">#line:042266c </span></span>
<span class="yarn-line">Look at this small CUB! <span class="yarn-meta">#line:0124e1c </span></span>
<span class="yarn-cmd">&lt;&lt;activity order_lion_settings lion_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-lion-activity-done"></a>
## lion_activity_done

<div class="yarn-node" data-title="lion_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: lion_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;inventory animal_lion add&gt;&gt;</span>
<span class="yarn-line">You must find the FLAG! <span class="yarn-meta">#line:05da6d7 </span></span>
<span class="yarn-line">I like watching it in wind. <span class="yarn-meta">#line:01b3593 </span></span>

</code></pre></div>

<a id="ys-node-lion-sign"></a>
## LION_SIGN

<div class="yarn-node" data-title="LION_SIGN"><pre class="yarn-code"><code><span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">tags: actor=Sign</span>
<span class="yarn-header-dim">image: lion_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">LION. PRIDE = family of lions. <span class="yarn-meta">#line:0ac6cc0 </span></span>
<span class="yarn-line">Males have MANES. <span class="yarn-meta">#line:0d2883b </span></span>

</code></pre></div>

<a id="ys-node-lion-kid"></a>
## LION_KID

<div class="yarn-node" data-title="LION_KID"><pre class="yarn-code"><code><span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">tags: actor=KID</span>
<span class="yarn-header-dim">image: kid_lion</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">What a roar! <span class="yarn-meta">#line:079f4e0 </span></span>

</code></pre></div>

<a id="ys-node-monkey-talk"></a>
## monkey_talk

<div class="yarn-node" data-title="monkey_talk"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">// PART 4 – MONKEY</span>
<span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: monkey_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I am a MONKEY. I climb TREES. <span class="yarn-meta">#line:0867233 </span></span>
<span class="yarn-line">We are close to HUMANS! <span class="yarn-meta">#line:0eaefd6 </span></span>
<span class="yarn-line">Where did I get these FRUITS? <span class="yarn-meta">#line:079451b </span></span>
<span class="yarn-cmd">&lt;&lt;activity match_monkey_settings monkey_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-monkey-activity-done"></a>
## monkey_activity_done

<div class="yarn-node" data-title="monkey_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: monkey_keeper</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;inventory animal_monkey add&gt;&gt;</span>
<span class="yarn-line">I did not take the FLAG. <span class="yarn-meta">#line:0c53945 </span></span>
<span class="yarn-line">It would be fun to climb that pole. <span class="yarn-meta">#line:0a43c85 </span></span>

</code></pre></div>

<a id="ys-node-monkey-sign"></a>
## MONKEY_SIGN

<div class="yarn-node" data-title="MONKEY_SIGN"><pre class="yarn-code"><code><span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">tags: actor=Sign</span>
<span class="yarn-header-dim">image: monkey_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">MONKEY. Great CLIMBER. <span class="yarn-meta">#line:0900219 </span></span>
<span class="yarn-line">Loves FRUIT. <span class="yarn-meta">#line:021b299 </span></span>

</code></pre></div>

<a id="ys-node-monkey-kid"></a>
## MONKEY_KID

<div class="yarn-node" data-title="MONKEY_KID"><pre class="yarn-code"><code><span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">tags: actor=KID</span>
<span class="yarn-header-dim">image: kid_monkey</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">He copied my dance! <span class="yarn-meta">#line:0bf2346 </span></span>
<span class="yarn-line">Do MONKEYS wink? <span class="yarn-meta">#line:0eecdf9 </span></span>

</code></pre></div>

<a id="ys-node-penguin-talk"></a>
## penguin_talk

<div class="yarn-node" data-title="penguin_talk"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">// PART 5 – PENGUIN</span>
<span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: penguin_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I am a PENGUIN, a strange BIRD. <span class="yarn-meta">#line:08c70e8 </span></span>
<span class="yarn-line">I can't fly, but I swim well! <span class="yarn-meta">#line:0540c5a </span></span>
<span class="yarn-line">Can you find the path in ICE? <span class="yarn-meta">#line:0a3420c </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_penguin_settings penguin_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-penguin-activity-done"></a>
## penguin_activity_done

<div class="yarn-node" data-title="penguin_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: penguin_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;inventory animal_penguin add&gt;&gt;</span>
<span class="yarn-line">No, I did not take the FLAG. <span class="yarn-meta">#line:078190f </span></span>
<span class="yarn-line">I can't fly, remember? <span class="yarn-meta">#line:08568f5 </span></span>

</code></pre></div>

<a id="ys-node-penguin-sign"></a>
## PENGUIN_SIGN

<div class="yarn-node" data-title="PENGUIN_SIGN"><pre class="yarn-code"><code><span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">tags: actor=Tutor</span>
<span class="yarn-header-dim">image: penguin_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">PENGUIN. Bird. Swimmer. <span class="yarn-meta">#line:0877d95 </span></span>
<span class="yarn-line">Lives near OCEANS. <span class="yarn-meta">#line:0eac350 </span></span>

</code></pre></div>

<a id="ys-node-penguin-kid"></a>
## PENGUIN_KID

<div class="yarn-node" data-title="PENGUIN_KID"><pre class="yarn-code"><code><span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">tags: actor=KID</span>
<span class="yarn-header-dim">image: kid_penguin</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">How cute! <span class="yarn-meta">#line:0ae73f3 </span></span>
<span class="yarn-line">It looks neat. <span class="yarn-meta">#line:05ac327 </span></span>

</code></pre></div>

<a id="ys-node-task-animals-done"></a>
## task_animals_done

<div class="yarn-node" data-title="task_animals_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">TASK DONE! Go back to the DIRECTOR. <span class="yarn-meta">#line:0a93d9b </span></span>
<span class="yarn-cmd">&lt;&lt;set $talked_animals = true&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-director-task-done"></a>
## director_task_done

<div class="yarn-node" data-title="director_task_done"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: actor=Man</span>
<span class="yarn-header-dim">image: centennial_hall_empty_flag</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Good. You talked to all ANIMALS. <span class="yarn-meta">#line:032811f </span></span>
<span class="yarn-line">Now let's review facts. <span class="yarn-meta">#line:0364f30 </span></span>
<span class="yarn-line">Help me put pieces together. <span class="yarn-meta">#line:08de86f </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_zoo_settings director_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-recap-cards"></a>
## RECAP_CARDS

<div class="yarn-node" data-title="RECAP_CARDS"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Icebox</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: zoo_cards</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Now match the CARDS. <span class="yarn-meta">#line:0f6f882 </span></span>
<span class="yarn-cmd">&lt;&lt;activity memory zoo_animal_cards tutorial&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-director-activity-done"></a>
## director_activity_done

<div class="yarn-node" data-title="director_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: zoo_recap</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">All ANIMALS are innocent. <span class="yarn-meta">#line:0bc2b46 </span></span>
<span class="yarn-line">Who took the FLAG? <span class="yarn-meta">#line:0fc0ab7 </span></span>
<span class="yarn-cmd">&lt;&lt;SetActive Antura with Flag&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump RETURN_DIRECTOR&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-return-director"></a>
## RETURN_DIRECTOR

<div class="yarn-node" data-title="RETURN_DIRECTOR"><pre class="yarn-code"><code><span class="yarn-header-dim">// TWIST – FLAG RETURN</span>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=ZooDirector</span>
<span class="yarn-header-dim">image: centennial_hall_antura_flag</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wait, look! <span class="yarn-meta">#line:0b3d05f </span></span>
<span class="yarn-cmd">&lt;&lt;camera_focus Antura with Flag&gt;&gt;</span>
<span class="yarn-line">ANTURA has the FLAG! <span class="yarn-meta">#line:0e24973 </span></span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump CEREMONY_END&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-ceremony-end"></a>
## CEREMONY_END

<div class="yarn-node" data-title="CEREMONY_END"><pre class="yarn-code"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=ZooDirector</span>
<span class="yarn-header-dim">image: flag_on_iglica</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The FLAG is home. <span class="yarn-meta">#line:0d91701 </span></span>
<span class="yarn-line">Thank you, helper. <span class="yarn-meta">#line:08c71db </span></span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code></pre></div>


