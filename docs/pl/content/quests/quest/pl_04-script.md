---
title: Zoo (pl_04) - Script
hide:
---

# Zoo (pl_04) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-04-the-zoo/35/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=819047762#gid=819047762)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_04%20Zoo/PL_04%20Zoo%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_04 | Zoo (Wroclaw)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: actor=Man, </span>
<span class="yarn-header-dim">image: centennial_hall_empty_flag</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $talked_animals = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $elephant_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $giraffe_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $lion_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $monkey_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $penguin_completed = false&gt;&gt;</span>
<span class="yarn-line">Witamy w ZOO WE WROCŁAWIU.</span> <span class="yarn-meta">#line:0fe55d1 </span>
<span class="yarn-line">Tutaj jest wiele ZWIERZĄT!</span> <span class="yarn-meta">#line:005dd46 </span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">To zadanie zostało ukończone.</span> <span class="yarn-meta">#line:0bcc257 </span>
<span class="yarn-line">Dowiedziałeś się o ZWIERZĘTACH ZOOLOGICZNYCH.</span> <span class="yarn-meta">#line:054cc37 </span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Narysuj swoje ulubione ZWIERZĘ.</span> <span class="yarn-meta">#line:0809ac5 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-director-talk"></a>

## director_talk

<div class="yarn-node" data-title="director_talk">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: actor=Man, type:Panel</span>
<span class="yarn-header-dim">image: centennial_hall_empty_flag</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $talked_animals == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump director_task_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump director_task&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-director-task"></a>

## director_task

<div class="yarn-node" data-title="director_task">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: actor=Man, type:Panel</span>
<span class="yarn-header-dim">image: centennial_hall_empty_flag</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Oh no!]</span> <span class="yarn-meta">#line:0c71176 </span>
<span class="yarn-cmd">&lt;&lt;camera_focus Flagpole&gt;&gt;</span>
<span class="yarn-line">O nie! FLAGA zaginęła!</span> <span class="yarn-meta">#line:09c6bf7 </span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">Miało to miejsce na IGLICA w CENTENNIAL HALL.</span> <span class="yarn-meta">#line:02f35e4 </span>
<span class="yarn-cmd">&lt;&lt;card iglica zoom&gt;&gt;</span>
<span class="yarn-line">Proszę pomóż mi to znaleźć!</span> <span class="yarn-meta">#line:0fd5d1a </span>
<span class="yarn-line">Porozmawiaj ze ZWIERZĘTAMI. Jedno może je mieć.</span> <span class="yarn-meta">#line:012b933 </span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_ANIMALS task_animals_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-animals-desc"></a>

## task_animals_desc

<div class="yarn-node" data-title="task_animals_desc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Znajdź FLAGĘ!</span> <span class="yarn-meta">#line:0da284c </span>
<span class="yarn-line">Porozmawiaj ze wszystkimi zwierzętami. Może wiedzą, gdzie jest flaga.</span> <span class="yarn-meta">#line:010adc5 </span>

</code>
</pre>
</div>

<a id="ys-node-animal-peacock"></a>

## animal_peacock

<div class="yarn-node" data-title="animal_peacock">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// ENTRANCE – AMBIENCE</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: ZOO</span>
<span class="yarn-header-dim">tags: actor=Peacock</span>
<span class="yarn-header-dim">image: zoo_gate</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Nie potrzebuję FLAG-u.</span> <span class="yarn-meta">#line:0085a8a </span>
<span class="yarn-line">Mój ogon to FLAGA!</span> <span class="yarn-meta">#line:04fff6b </span>

</code>
</pre>
</div>

<a id="ys-node-animal-parrot"></a>

## animal_parrot

<div class="yarn-node" data-title="animal_parrot">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ZOO</span>
<span class="yarn-header-dim">tags: actor=Parrot</span>
<span class="yarn-header-dim">image: zoo_gate</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Krzyk! Czerwony i żółty przebiegli!</span> <span class="yarn-meta">#line:0e84545 </span>

</code>
</pre>
</div>

<a id="ys-node-animal-fox"></a>

## animal_fox

<div class="yarn-node" data-title="animal_fox">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ZOO</span>
<span class="yarn-header-dim">tags: actor=Parrot</span>
<span class="yarn-header-dim">image: zoo_gate</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Hehe! I'm not actually from the zoo.]</span> <span class="yarn-meta">#line:03c4553 </span>
<span class="yarn-line">[MISSING TRANSLATION: I didn't see a flag anywhere.]</span> <span class="yarn-meta">#line:074fe29 </span>

</code>
</pre>
</div>

<a id="ys-node-item-chest"></a>

## item_chest

<div class="yarn-node" data-title="item_chest">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">group: actions</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: What a nice surprise!]</span> <span class="yarn-meta">#line:03321d5 </span>
<span class="yarn-cmd">&lt;&lt;action open_chest&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-elephant-talk"></a>

## elephant_talk

<div class="yarn-node" data-title="elephant_talk">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// PART 1 – ELEPHANT</span>
<span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: elephant_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $elephant_completed&gt;&gt;</span>
<span class="yarn-line">    [MISSING TRANSLATION:     A FLAG? I do not have it.]</span> <span class="yarn-meta">#line:03879f8 </span>
<span class="yarn-line">    [MISSING TRANSLATION:     If I took it I would remember!]</span> <span class="yarn-meta">#line:0103606 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jestem największym ZWIERZĘCIEM lądowym.</span> <span class="yarn-meta">#line:027b51f </span>
    <span class="yarn-cmd">&lt;&lt;card animal_elephant zoom&gt;&gt;</span>
<span class="yarn-line">    Mam dobrą PAMIĘĆ.</span> <span class="yarn-meta">#line:03a150c </span>
<span class="yarn-line">    Czy masz dobrą PAMIĘĆ?</span> <span class="yarn-meta">#line:0f98478 </span>
    <span class="yarn-cmd">&lt;&lt;activity memory_elephant_settings elephant_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-elephant-activity-done"></a>

## elephant_activity_done

<div class="yarn-node" data-title="elephant_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: elephant_keeper</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;inventory animal_elephant add&gt;&gt;</span>
<span class="yarn-line">FLAGA? Nie mam jej.</span> <span class="yarn-meta">#line:0b79d01 </span>
<span class="yarn-line">Gdybym wziął, to bym pamiętał!</span> <span class="yarn-meta">#line:0f124bf </span>
<span class="yarn-cmd">&lt;&lt;set $elephant_completed = true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-elephant-sign"></a>

## ELEPHANT_SIGN

<div class="yarn-node" data-title="ELEPHANT_SIGN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">tags: actor=tutor</span>
<span class="yarn-header-dim">image: elephant_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">SŁOŃ. Gruba skóra. Duże USZY.</span> <span class="yarn-meta">#line:048e8a1 </span>
<span class="yarn-line">Największe ZWIERZĘ LĄDOWE.</span> <span class="yarn-meta">#line:0b1cca2 </span>

</code>
</pre>
</div>

<a id="ys-node-elephant-kid"></a>

## ELEPHANT_KID

<div class="yarn-node" data-title="ELEPHANT_KID">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">tags: actor=KID</span>
<span class="yarn-header-dim">image: kid_elephant</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jego USZY są większe ode mnie!</span> <span class="yarn-meta">#line:0fc78ad </span>
<span class="yarn-line">Czy może mnie wachlować latem?</span> <span class="yarn-meta">#line:004abc7 </span>


</code>
</pre>
</div>

<a id="ys-node-giraffe-talk"></a>

## giraffe_talk

<div class="yarn-node" data-title="giraffe_talk">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// PART 2 – GIRAFFE</span>
<span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: giraffe_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $giraffe_completed&gt;&gt;</span>
<span class="yarn-line">    [MISSING TRANSLATION:     I did not take the FLAG.]</span> <span class="yarn-meta">#line:05d1f0a </span>
<span class="yarn-line">    [MISSING TRANSLATION:     It is too high for me!]</span> <span class="yarn-meta">#line:04947e6 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jestem najwyższym ZWIERZĘCIEM.</span> <span class="yarn-meta">#line:0d5c607 </span>
    <span class="yarn-cmd">&lt;&lt;card animal_giraffe zoom&gt;&gt;</span>
<span class="yarn-line">    Moja długa SZYJA pomaga mi dosięgnąć liści.</span> <span class="yarn-meta">#line:0a4d24e </span>
<span class="yarn-line">    Jem liście akacji!</span> <span class="yarn-meta">#line:04b42f2 </span>
    <span class="yarn-cmd">&lt;&lt;activity canvas_giraffe_settings giraffe_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-giraffe-activity-done"></a>

## giraffe_activity_done

<div class="yarn-node" data-title="giraffe_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: giraffe_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;inventory animal_elephant add&gt;&gt;</span>
<span class="yarn-line">Nie wziąłem FLAG-a.</span> <span class="yarn-meta">#line:0877d6f </span>
<span class="yarn-line">To jest dla mnie za wysoko!</span> <span class="yarn-meta">#line:02d00e2 </span>
<span class="yarn-cmd">&lt;&lt;set $giraffe_completed = true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-giraffe-sign"></a>

## GIRAFFE_SIGN

<div class="yarn-node" data-title="GIRAFFE_SIGN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">tags: actor=Sign</span>
<span class="yarn-header-dim">image: giraffe_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">ŻYRAFA. Wysoka. Długa szyja.</span> <span class="yarn-meta">#line:0a8a73f </span>
<span class="yarn-line">Długie RZĘSY.</span> <span class="yarn-meta">#line:0291317 </span>

</code>
</pre>
</div>

<a id="ys-node-giraffe-kid"></a>

## GIRAFFE_KID

<div class="yarn-node" data-title="GIRAFFE_KID">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">tags: actor=KID</span>
<span class="yarn-header-dim">image: kid_giraffe</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Z taką SZYJĄ!</span> <span class="yarn-meta">#line:068daeb </span>
<span class="yarn-line">Stąd mogłem widzieć swój dom.</span> <span class="yarn-meta">#line:0bee484 </span>


</code>
</pre>
</div>

<a id="ys-node-lion-talk"></a>

## lion_talk

<div class="yarn-node" data-title="lion_talk">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// PART 3 – LION</span>
<span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: lion_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $lion_completed&gt;&gt;</span>
<span class="yarn-line">    [MISSING TRANSLATION:     You must find the FLAG!]</span> <span class="yarn-meta">#line:0b1109f </span>
<span class="yarn-line">    [MISSING TRANSLATION:     I like watching it in wind.]</span> <span class="yarn-meta">#line:095bf3c </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jestem LWEM w AFRYCE.</span> <span class="yarn-meta">#line:07f2e15 </span>
    <span class="yarn-cmd">&lt;&lt;card  animal_lion zoom&gt;&gt;</span>
<span class="yarn-line">    Żyję w DUMIE.</span> <span class="yarn-meta">#line:042266c </span>
<span class="yarn-line">    Spójrz na to małe CUB!</span> <span class="yarn-meta">#line:0124e1c </span>
    <span class="yarn-cmd">&lt;&lt;card  animal_lion_cub zoom&gt;&gt;</span>
<span class="yarn-line">    [MISSING TRANSLATION:     I grow up small, then medium, then old...]</span> <span class="yarn-meta">#line:0774b92 </span>
    <span class="yarn-cmd">&lt;&lt;activity order_lion_settings lion_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-lion-activity-done"></a>

## lion_activity_done

<div class="yarn-node" data-title="lion_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: lion_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;inventory animal_lion add&gt;&gt;</span>
<span class="yarn-line">Musisz znaleźć FLAGĘ!</span> <span class="yarn-meta">#line:05da6d7 </span>
<span class="yarn-line">Lubię patrzeć na niego na wietrze.</span> <span class="yarn-meta">#line:01b3593 </span>
<span class="yarn-cmd">&lt;&lt;set $lion_completed = true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-lion-sign"></a>

## LION_SIGN

<div class="yarn-node" data-title="LION_SIGN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">tags: actor=Sign</span>
<span class="yarn-header-dim">image: lion_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">LEW. DUMA = rodzina lwów.</span> <span class="yarn-meta">#line:0ac6cc0 </span>
<span class="yarn-line">Samce mają GRZYWY.</span> <span class="yarn-meta">#line:0d2883b </span>

</code>
</pre>
</div>

<a id="ys-node-lion-kid"></a>

## LION_KID

<div class="yarn-node" data-title="LION_KID">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">tags: actor=KID</span>
<span class="yarn-header-dim">image: kid_lion</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cóż za ryk!</span> <span class="yarn-meta">#line:079f4e0 </span>

</code>
</pre>
</div>

<a id="ys-node-monkey-talk"></a>

## monkey_talk

<div class="yarn-node" data-title="monkey_talk">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// PART 4 – MONKEY</span>
<span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: monkey_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $monkey_completed&gt;&gt;</span>
<span class="yarn-line">    [MISSING TRANSLATION:     I did not take the FLAG.]</span> <span class="yarn-meta">#line:092a68e </span>
<span class="yarn-line">    [MISSING TRANSLATION:     It would be fun to climb that pole.]</span> <span class="yarn-meta">#line:097810d </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jestem MAŁPĄ. Wspinam się na drzewa.</span> <span class="yarn-meta">#line:0867233 </span>
    <span class="yarn-cmd">&lt;&lt;card animal_chimpanzee zoom&gt;&gt;</span>
<span class="yarn-line">    Jesteśmy blisko LUDZI!</span> <span class="yarn-meta">#line:0eaefd6 </span>
<span class="yarn-line">    Gdzie dostałem te OWOCE?</span> <span class="yarn-meta">#line:079451b </span>
    <span class="yarn-cmd">&lt;&lt;activity match_monkey_settings monkey_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-monkey-activity-done"></a>

## monkey_activity_done

<div class="yarn-node" data-title="monkey_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: monkey_keeper</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;inventory animal_monkey add&gt;&gt;</span>
<span class="yarn-line">Nie wziąłem FLAG-a.</span> <span class="yarn-meta">#line:0c53945 </span>
<span class="yarn-line">Byłoby fajnie wspiąć się na ten słup.</span> <span class="yarn-meta">#line:0a43c85</span>
<span class="yarn-cmd">&lt;&lt;set $monkey_completed = true&gt;&gt;</span> 

</code>
</pre>
</div>

<a id="ys-node-monkey-sign"></a>

## MONKEY_SIGN

<div class="yarn-node" data-title="MONKEY_SIGN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">tags: actor=Sign</span>
<span class="yarn-header-dim">image: monkey_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">MAŁPA. Świetny WSPINACZ.</span> <span class="yarn-meta">#line:0900219 </span>
<span class="yarn-line">Uwielbia OWOCE.</span> <span class="yarn-meta">#line:021b299 </span>

</code>
</pre>
</div>

<a id="ys-node-monkey-kid"></a>

## MONKEY_KID

<div class="yarn-node" data-title="MONKEY_KID">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">tags: actor=KID</span>
<span class="yarn-header-dim">image: kid_monkey</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Skopiował mój taniec!</span> <span class="yarn-meta">#line:0bf2346 </span>
<span class="yarn-line">Czy MAŁPY mrugają?</span> <span class="yarn-meta">#line:0eecdf9 </span>

</code>
</pre>
</div>

<a id="ys-node-penguin-talk"></a>

## penguin_talk

<div class="yarn-node" data-title="penguin_talk">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// PART 5 – PENGUIN</span>
<span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: penguin_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $penguin_completed&gt;&gt;</span>
<span class="yarn-line">    [MISSING TRANSLATION:     No, I did not take the FLAG.]</span> <span class="yarn-meta">#line:0b71fae </span>
<span class="yarn-line">    [MISSING TRANSLATION:     I can't fly, remember?]</span> <span class="yarn-meta">#line:03e6fc7 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jestem PINGWINEM, dziwnym PTAKEM.</span> <span class="yarn-meta">#line:08c70e8 </span>
    <span class="yarn-cmd">&lt;&lt;card animal_penguin zoom&gt;&gt;</span>
<span class="yarn-line">    Nie umiem latać, ale dobrze pływam!</span> <span class="yarn-meta">#line:0540c5a </span>
<span class="yarn-line">    Czy potrafisz znaleźć ścieżkę w ICE?</span> <span class="yarn-meta">#line:0a3420c </span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_penguin_settings penguin_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-penguin-activity-done"></a>

## penguin_activity_done

<div class="yarn-node" data-title="penguin_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">tags: actor=Keeper</span>
<span class="yarn-header-dim">image: penguin_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;inventory animal_penguin add&gt;&gt;</span>
<span class="yarn-line">Nie, nie wziąłem FLAG-a.</span> <span class="yarn-meta">#line:078190f </span>
<span class="yarn-line">Nie potrafię latać, pamiętasz?</span> <span class="yarn-meta">#line:08568f5 </span>
<span class="yarn-cmd">&lt;&lt;set $penguin_completed = true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-penguin-sign"></a>

## PENGUIN_SIGN

<div class="yarn-node" data-title="PENGUIN_SIGN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">tags: actor=Tutor</span>
<span class="yarn-header-dim">image: penguin_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">PINGWIN. Ptak. Pływak.</span> <span class="yarn-meta">#line:0877d95 </span>
<span class="yarn-line">Mieszka w pobliżu OCEANÓW.</span> <span class="yarn-meta">#line:0eac350 </span>

</code>
</pre>
</div>

<a id="ys-node-penguin-kid"></a>

## PENGUIN_KID

<div class="yarn-node" data-title="PENGUIN_KID">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">tags: actor=KID</span>
<span class="yarn-header-dim">image: kid_penguin</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jakie słodkie!</span> <span class="yarn-meta">#line:0ae73f3 </span>
<span class="yarn-line">Wygląda fajnie.</span> <span class="yarn-meta">#line:05ac327 </span>

</code>
</pre>
</div>

<a id="ys-node-task-animals-done"></a>

## task_animals_done

<div class="yarn-node" data-title="task_animals_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">ZADANIE WYKONANE! Wróć do DYREKTORA.</span> <span class="yarn-meta">#line:0a93d9b </span>
<span class="yarn-cmd">&lt;&lt;set $talked_animals = true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-director-task-done"></a>

## director_task_done

<div class="yarn-node" data-title="director_task_done">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">tags: actor=Man</span>
<span class="yarn-header-dim">image: centennial_hall_empty_flag</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dobrze. Rozmawiałeś ze WSZYSTKIMI ZWIERZĘTAMI.</span> <span class="yarn-meta">#line:032811f </span>
<span class="yarn-line">Teraz przyjrzyjmy się faktom.</span> <span class="yarn-meta">#line:0364f30 </span>
<span class="yarn-line">Pomóż mi złożyć wszystko w całość.</span> <span class="yarn-meta">#line:08de86f </span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_zoo_settings director_activity_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-recap-cards"></a>

## RECAP_CARDS

<div class="yarn-node" data-title="RECAP_CARDS">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Icebox</span>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: zoo_cards</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Teraz dopasuj KARTY.</span> <span class="yarn-meta">#line:0f6f882 </span>
<span class="yarn-cmd">&lt;&lt;activity memory zoo_animal_cards tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-director-activity-done"></a>

## director_activity_done

<div class="yarn-node" data-title="director_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: actor=Narrator</span>
<span class="yarn-header-dim">image: zoo_recap</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wszystkie ZWIERZĘTA są niewinne.</span> <span class="yarn-meta">#line:0bc2b46 </span>
<span class="yarn-line">Kto wziął FLAGĘ?</span> <span class="yarn-meta">#line:0fc0ab7 </span>
<span class="yarn-cmd">&lt;&lt;SetActive anturaFlag&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump RETURN_DIRECTOR&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-return-director"></a>

## RETURN_DIRECTOR

<div class="yarn-node" data-title="RETURN_DIRECTOR">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// TWIST – FLAG RETURN</span>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=ZooDirector</span>
<span class="yarn-header-dim">image: centennial_hall_antura_flag</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Poczekaj, spójrz!</span> <span class="yarn-meta">#line:0b3d05f </span>
<span class="yarn-cmd">&lt;&lt;camera_focus Antura&gt;&gt;</span>
<span class="yarn-line">ANTURA ma FLAGĘ!</span> <span class="yarn-meta">#line:0e24973 </span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump CEREMONY_END&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ceremony-end"></a>

## CEREMONY_END

<div class="yarn-node" data-title="CEREMONY_END">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">tags: actor=ZooDirector</span>
<span class="yarn-header-dim">image: flag_on_iglica</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">FLAG jest domem.</span> <span class="yarn-meta">#line:0d91701 </span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_flag&gt;&gt;</span>
<span class="yarn-line">Dziękuję, pomocniku.</span> <span class="yarn-meta">#line:08c71db </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>


