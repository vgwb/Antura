---
title: The Zoo (pl_04) - Script
hide:
---

# The Zoo (pl_04) - Script
[Quest Index](./index.md) - Language: english - [french](./pl_04-script.fr.md) - [polish](./pl_04-script.pl.md) - [italian](./pl_04-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/pl-04-the-zoo/35/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1233127135#gid=1233127135)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_04%20Zoo/PL_04%20Zoo%20-%20Yarn%20Script.yarn)  

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// PL_04 – Wroclaw Zoo</span>
<span class="yarn-header-dim">// Activities:</span>
<span class="yarn-header-dim">// 1) &lt;&lt;activity memory elephant_sounds tutorial&gt;&gt; – Repeat sound order (ELEPHANT, MEMORY).</span>
<span class="yarn-header-dim">// 2) &lt;&lt;activity cleancanvas giraffe_leaves tutorial&gt;&gt; – Clean to help EAT (GIRAFFE, NECK, LEAVES).</span>
<span class="yarn-header-dim">// 3) &lt;&lt;activity order lion_pride tutorial&gt;&gt; – Match LION facts (PRIDE, MANE, CUB).</span>
<span class="yarn-header-dim">// 4) &lt;&lt;activity cleancanvas monkey_climb tutorial&gt;&gt; – Clean climb path (MONKEY, CLIMB).</span>
<span class="yarn-header-dim">// 5) &lt;&lt;activity memory penguin_paths tutorial&gt;&gt; – Remember icy path (PENGUIN, SWIM).</span>
<span class="yarn-header-dim">// 6) &lt;&lt;activity order zoo_animal_facts tutorial&gt;&gt; – Pair ANIMALS ↔ FACTS (recap).</span>
<span class="yarn-header-dim">// 7) &lt;&lt;activity memory zoo_animal_cards tutorial&gt;&gt; – Animal card pairs (recap).</span>
<span class="yarn-header-dim">// 8) &lt;&lt;activity quiz wroclaw_zoo_basics tutorial&gt;&gt; – Final quiz (FLAG, facts).</span>
<span class="yarn-header-dim">// Words:</span>
<span class="yarn-header-dim">// POLAND, WROCŁAW, ZOO, FLAG, CENTENNIAL HALL, IGLICA, ANIMAL, ELEPHANT, GIRAFFE, LION, MONKEY, PENGUIN, PRIDE, CLIMB, SWIM, KEEPER, SIGN</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: actor=Man, type:Panel</span>
<span class="yarn-header-dim">image: centennial_hall_empty_flag</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $talked_animals = false&gt;&gt;</span>
<span class="yarn-line">TUTOR: Welcome to Wroclaw Zoo. <span class="yarn-meta">#line:0fe55d1 </span></span>
<span class="yarn-line">There are lots of animals here! <span class="yarn-meta">#line:005dd46 </span></span>

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
<span class="yarn-line">It was on the IGLICA at CENTENNIAL HALL. <span class="yarn-meta">#line:02f35e4 </span></span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">Please help me, find it! <span class="yarn-meta">#line:0fd5d1a </span></span>
<span class="yarn-line">Talk to the animals, maybe one of them took it. <span class="yarn-meta">#line:012b933 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_ANIMALS task_animals_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-animals-desc"></a>
## task_animals_desc

<div class="yarn-node" data-title="task_animals_desc"><pre class="yarn-code"><code><span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Find the flag! <span class="yarn-meta">#line:0da284c </span></span>
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
<span class="yarn-line">I don't need a flag, <span class="yarn-meta">#line:0085a8a </span></span>
<span class="yarn-line">my tail is a FLAG already! <span class="yarn-meta">#line:04fff6b </span></span>

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

<div class="yarn-node" data-title="elephant_talk"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// PART 1 – ELEPHANT</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: elephant_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I'm an elephant, the biggest animal on land. <span class="yarn-meta">#line:027b51f </span></span>
<span class="yarn-line">I have great memory. <span class="yarn-meta">#line:03a150c </span></span>
<span class="yarn-line">Do you have a good memory too? <span class="yarn-meta">#line:0f98478 </span></span>
<span class="yarn-cmd">&lt;&lt;activity memory_elephant_settings elephant_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-elephant-activity-done"></a>
## elephant_activity_done

<div class="yarn-node" data-title="elephant_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: elephant_keeper</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;inventory animal_elephant add&gt;&gt;</span>
<span class="yarn-line">A flag? I don't have one. <span class="yarn-meta">#line:0b79d01 </span></span>
<span class="yarn-line">If I had taken it, I would remember! <span class="yarn-meta">#line:0f124bf </span></span>

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
<span class="yarn-header-dim">tags: actor=Kid</span>
<span class="yarn-header-dim">image: kid_elephant</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Its ears are bigger than me! <span class="yarn-meta">#line:0fc78ad </span></span>
<span class="yarn-line">Can it fan me in the summer? <span class="yarn-meta">#line:004abc7 </span></span>

<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// PART 2 – GIRAFFE</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-giraffe-talk"></a>
## giraffe_talk

<div class="yarn-node" data-title="giraffe_talk"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: giraffe_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I'm a giraffe, the tallest of the animals. <span class="yarn-meta">#line:0d5c607 </span></span>
<span class="yarn-line">My long neck help me reach the leaves. <span class="yarn-meta">#line:0a4d24e </span></span>
<span class="yarn-line">Do you want to watch me eat? <span class="yarn-meta">#line:04b42f2 </span></span>
<span class="yarn-cmd">&lt;&lt;activity canvas_giraffe_settings giraffe_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-giraffe-activity-done"></a>
## giraffe_activity_done

<div class="yarn-node" data-title="giraffe_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: giraffe_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;inventory animal_elephant add&gt;&gt;</span>
<span class="yarn-line">I didn't take the flag, <span class="yarn-meta">#line:0877d6f </span></span>
<span class="yarn-line">it's too tall for even me to reach! <span class="yarn-meta">#line:02d00e2 </span></span>

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
<span class="yarn-header-dim">tags: actor=Kid</span>
<span class="yarn-header-dim">image: kid_giraffe</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">With a neck like that, <span class="yarn-meta">#line:068daeb </span></span>
<span class="yarn-line">I could see my house even from here. <span class="yarn-meta">#line:0bee484 </span></span>

<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// PART 3 – LION</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-lion-talk"></a>
## lion_talk

<div class="yarn-node" data-title="lion_talk"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: lion_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I'm a lion, the biggest predator in Africa. <span class="yarn-meta">#line:07f2e15 </span></span>
<span class="yarn-line">I live in groups with other lions. <span class="yarn-meta">#line:042266c </span></span>
<span class="yarn-line">How well do you know lions? <span class="yarn-meta">#line:0124e1c </span></span>
<span class="yarn-cmd">&lt;&lt;activity order_lion_settings lion_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-lion-activity-done"></a>
## lion_activity_done

<div class="yarn-node" data-title="lion_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: lion_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;inventory animal_lion add&gt;&gt;</span>
<span class="yarn-line">You have to find the flag! <span class="yarn-meta">#line:05da6d7 </span></span>
<span class="yarn-line">I like watching it flutter in the wind. <span class="yarn-meta">#line:01b3593 </span></span>

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
<span class="yarn-header-dim">tags: actor=Kid</span>
<span class="yarn-header-dim">image: kid_lion</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">What a roar! <span class="yarn-meta">#line:079f4e0 </span></span>

<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// PART 4 – MONKEY</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-monkey-talk"></a>
## monkey_talk

<div class="yarn-node" data-title="monkey_talk"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: monkey_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I'm an ape, and I love to climb trees. <span class="yarn-meta">#line:0867233 </span></span>
<span class="yarn-line">Did you know? I'm related to humans! <span class="yarn-meta">#line:0eaefd6 </span></span>
<span class="yarn-line">Clean the path so I can climb. <span class="yarn-meta">#line:079451b </span></span>
<span class="yarn-cmd">&lt;&lt;activity match_monkey_settings monkey_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-monkey-activity-done"></a>
## monkey_activity_done

<div class="yarn-node" data-title="monkey_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: monkey_keeper</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;inventory animal_monkey add&gt;&gt;</span>
<span class="yarn-line">I'm not the one who took the flag. <span class="yarn-meta">#line:0c53945 </span></span>
<span class="yarn-line">But it would be fun to climb that pole... <span class="yarn-meta">#line:0a43c85 </span></span>

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
<span class="yarn-header-dim">tags: actor=Kid</span>
<span class="yarn-header-dim">image: kid_monkey</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">He copied my dance! <span class="yarn-meta">#line:0bf2346 </span></span>
<span class="yarn-line">Do monkeys wink? <span class="yarn-meta">#line:0eecdf9 </span></span>

<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// PART 5 – PENGUIN</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-penguin-talk"></a>
## penguin_talk

<div class="yarn-node" data-title="penguin_talk"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: penguin_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I'm a penguin, and I'm a strange bird. <span class="yarn-meta">#line:08c70e8 </span></span>
<span class="yarn-line">I can't fly, but I can swim really well!. <span class="yarn-meta">#line:0540c5a </span></span>
<span class="yarn-line">Can you rememeber the right path through the ice? <span class="yarn-meta">#line:0a3420c </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_penguin_settings penguin_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-penguin-activity-done"></a>
## penguin_activity_done

<div class="yarn-node" data-title="penguin_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: penguin_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;inventory animal_penguin add&gt;&gt;</span>
<span class="yarn-line">No, I didn't take the flag. <span class="yarn-meta">#line:078190f </span></span>
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
<span class="yarn-header-dim">tags: actor=Kid</span>
<span class="yarn-header-dim">image: kid_penguin</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">How cute! <span class="yarn-meta">#line:0ae73f3 </span></span>
<span class="yarn-line">It looks like it's wearing a tiny tuxedo. <span class="yarn-meta">#line:05ac327 </span></span>

<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// RECAP – MATCH &amp; CARDS</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-task-animals-done"></a>
## task_animals_done

<div class="yarn-node" data-title="task_animals_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">TUTOR: TASK COMPLETED! Go back to the director. <span class="yarn-meta">#line:0a93d9b </span></span>
<span class="yarn-cmd">&lt;&lt;set $talked_animals = true&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-director-task-done"></a>
## director_task_done

<div class="yarn-node" data-title="director_task_done"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: actor=Man, type:Panel</span>
<span class="yarn-header-dim">image: centennial_hall_empty_flag</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Good, you talked to all the animals. <span class="yarn-meta">#line:032811f </span></span>
<span class="yarn-line">Now let's review these facts. <span class="yarn-meta">#line:0364f30 </span></span>
<span class="yarn-line">Help me put the pieces toghether.  <span class="yarn-meta">#line:08de86f </span></span>
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
<span class="yarn-line">It looks like the animals are all innocent. <span class="yarn-meta">#line:0bc2b46 </span></span>
<span class="yarn-line">But then, who took the flag? <span class="yarn-meta">#line:0fc0ab7 </span></span>
<span class="yarn-cmd">&lt;&lt;SetActive Antura with Flag&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump RETURN_DIRECTOR&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-return-director"></a>
## RETURN_DIRECTOR

<div class="yarn-node" data-title="RETURN_DIRECTOR"><pre class="yarn-code"><code><span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// TWIST – FLAG RETURN</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=Zoo Director</span>
<span class="yarn-header-dim">image: centennial_hall_antura_flag</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wait, Look!  <span class="yarn-meta">#line:0b3d05f </span></span>
<span class="yarn-cmd">&lt;&lt;camera_focus Antura with Flag&gt;&gt;</span>
<span class="yarn-line">ANTURA has the FLAG! <span class="yarn-meta">#line:0e24973 </span></span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump CEREMONY_END&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-ceremony-end"></a>
## CEREMONY_END

<div class="yarn-node" data-title="CEREMONY_END"><pre class="yarn-code"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=Zoo Director</span>
<span class="yarn-header-dim">image: flag_on_iglica</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The FLAG is finally home. <span class="yarn-meta">#line:0d91701 </span></span>
<span class="yarn-line">Thank you, detective. <span class="yarn-meta">#line:08c71db </span></span>
<span class="yarn-cmd">&lt;&lt;jump QUEST_COMPLETE&gt;&gt;</span>

<span class="yarn-comment">//--------------------------------------------</span>
<span class="yarn-comment">// FINAL QUIZ</span>
<span class="yarn-comment">//--------------------------------------------</span>

</code></pre></div>

<a id="ys-node-final-quiz"></a>
## FINAL_QUIZ

<div class="yarn-node" data-title="FINAL_QUIZ"><pre class="yarn-code"><code><span class="yarn-header-dim">group: Icebox</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: wroclaw_flag_quiz</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Answer the questions. <span class="yarn-meta">#line:010b191 </span></span>
<span class="yarn-cmd">&lt;&lt;activity quiz wroclaw_zoo_basics QUEST_COMPLETE&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-quest-complete"></a>
## QUEST_COMPLETE

<div class="yarn-node" data-title="QUEST_COMPLETE"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: quest_complete</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great work at the ZOO! <span class="yarn-meta">#line:056d51c </span></span>
<span class="yarn-line">See you on the ODRA RIVER! <span class="yarn-meta">#line:0d4f5f7 </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code></pre></div>


