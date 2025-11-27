---
title: Gingerbread & food market (pl_06) - Script
hide:
---

# Gingerbread & food market (pl_06) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-06-gingerbread-food-market/37/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1211829352#gid=1211829352)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_06%20Torun%20Market/PL_06%20Torun%20Market%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_06 | Market (Torun)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">// ---------</span>
<span class="yarn-header-dim">// WANTED:</span>
<span class="yarn-header-dim">// Cards:</span>
<span class="yarn-header-dim">// - torun_gingerbread (cultural tradition)</span>
<span class="yarn-header-dim">// - torun_town_hall (Gothic architecture)</span>
<span class="yarn-header-dim">// - medieval_market (historical setting)</span>
<span class="yarn-header-dim">// - pierogi</span>
<span class="yarn-header-dim">// Activities:</span>
<span class="yarn-header-dim">// - bake gingerbread (order/memory of ingredients)</span>
<span class="yarn-header-dim">// - Pierogi Challenge: order/memory of ingredients (flour, eggs, cheese, potatoes)</span>
<span class="yarn-header-dim">// Words used: Toruń, market, vendor, grocer, beekeeper, dairy, eggs, milk, butter, flour, honey, cloves, cinnamon, ginger, pierogi, molds, coins, zloty, kitchen, gingerbread, medieval</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $grocer_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $beekeeper_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $cheesemonger_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $eggvendor_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $spicevendor_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $gingerbread_done = false&gt;&gt;</span>

<span class="yarn-line">We are in TORUŃ, home of Poland's legendary GINGERBREAD!</span> <span class="yarn-meta">#line:080555e </span>
<span class="yarn-line">Let's explore the MARKET.</span> <span class="yarn-meta">#line:03e11fa </span>
<span class="yarn-cmd">&lt;&lt;target npc_cook&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This quest is complete.</span> <span class="yarn-meta">#line:073978d</span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Would you like to try making a GINGERBREAD?</span> <span class="yarn-meta">#line:09722f2 </span>
<span class="yarn-line">Or the PIEROGI recipe?</span> <span class="yarn-meta">#line:0954c65 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cook"></a>

## cook

<div class="yarn-node" data-title="cook">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetCollectedItem("COLLECT_INGREDIENTS") &gt;= 7&gt;&gt;
   <span class="yarn-cmd">&lt;&lt;jump cook_ingredients_done&gt;&gt;</span>
&lt;&lt;elseif GetCollectedItem("COLLECT_INGREDIENTS") &gt; 0&gt;&gt;
   <span class="yarn-cmd">&lt;&lt;jump cook_not_enough&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
   <span class="yarn-cmd">&lt;&lt;jump cook_welcome&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cook-welcome"></a>

## cook_welcome

<div class="yarn-node" data-title="cook_welcome">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card cook&gt;&gt;</span>
<span class="yarn-line">Dzień dobry! I am a COOK of typical Polish recipes.</span> <span class="yarn-meta">#line:028131b</span>
<span class="yarn-cmd">&lt;&lt;card gingerbread&gt;&gt;</span>
<span class="yarn-line">I can bake our famous GINGERBREAD for you.</span> <span class="yarn-meta">#line:07fb019</span>
<span class="yarn-line">But I need some INGREDIENTS.</span> <span class="yarn-meta">#line:00a46f0</span>
<span class="yarn-cmd">&lt;&lt;jump task_ingredients&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-ingredients"></a>

## task_ingredients

<div class="yarn-node" data-title="task_ingredients">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  currency_zloty&gt;&gt;</span>
<span class="yarn-line">Please take this money</span> <span class="yarn-meta">#line:0ff272d </span>
<span class="yarn-cmd">&lt;&lt;card market_traders&gt;&gt;</span>
<span class="yarn-line">And go to the MARKET TRADERS</span> <span class="yarn-meta">#line:0c35a9c</span>
<span class="yarn-cmd">&lt;&lt;detour task_ingredients_desc&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start COLLECT_INGREDIENTS task_ingredients_done&gt;&gt;</span>
<span class="yarn-line">When you talk to people, remember your manners!</span> <span class="yarn-meta">#line:03f1020 </span>
<span class="yarn-line">We say "Dzień dobry" to greet someone</span> <span class="yarn-meta">#line:091bb57 </span>
<span class="yarn-line">and "Dziękuję" to thank them.</span> <span class="yarn-meta">#line:076ffac </span>
<span class="yarn-cmd">&lt;&lt;area area_full&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-ingredients-desc"></a>

## task_ingredients_desc

<div class="yarn-node" data-title="task_ingredients_desc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Buy the ingredients for the recipe.</span> <span class="yarn-meta">#line:00565e5 </span>
<span class="yarn-line">Some FLOUR</span> <span class="yarn-meta">#line:070b733 </span>
<span class="yarn-line">Some HONEY</span> <span class="yarn-meta">#line:0e22cab </span>
<span class="yarn-line">Some SUGAR</span> <span class="yarn-meta">#line:055af31</span>
<span class="yarn-line">Some BUTTER</span> <span class="yarn-meta">#line:0bfb896 </span>
<span class="yarn-line">One EGG</span> <span class="yarn-meta">#line:0de2001 </span>
<span class="yarn-line">Some CINNAMON</span> <span class="yarn-meta">#line:0ba5cca </span>
<span class="yarn-line">And finally some GINGER</span> <span class="yarn-meta">#line:0a700e3 </span>
<span class="yarn-cmd">&lt;&lt;card_hide&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cook-not-enough"></a>

## cook_not_enough

<div class="yarn-node" data-title="cook_not_enough">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You don't have all INGREDIENTS!</span> <span class="yarn-meta">#line:003f24d </span>
<span class="yarn-line">Visit all the VENDORS in the MARKET.</span> <span class="yarn-meta">#line:0d691b9 </span>
<span class="yarn-line">And take the ingredients you buy.</span> <span class="yarn-meta">#line:0ea58f3 </span>

</code>
</pre>
</div>

<a id="ys-node-task-ingredients-done"></a>

## task_ingredients_done

<div class="yarn-node" data-title="task_ingredients_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">type:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great. You got all INGREDIENTS!</span> <span class="yarn-meta">#line:03f7a4a</span>
<span class="yarn-cmd">&lt;&lt;target npc_cook&gt;&gt;</span>
<span class="yarn-line">Go back to the COOK.</span> <span class="yarn-meta">#line:0222915 </span>
<span class="yarn-cmd">&lt;&lt;task_start go_back_cook&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cook-ingredients-done"></a>

## cook_ingredients_done

<div class="yarn-node" data-title="cook_ingredients_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Well done, you have everything.</span> <span class="yarn-meta">#line:0bb1792</span>
<span class="yarn-line">And you were very polite.</span> <span class="yarn-meta">#line:0c400be </span>
<span class="yarn-cmd">&lt;&lt;card gingerbread&gt;&gt;</span>
<span class="yarn-line">Now we can bake TORUŃ GINGERBREAD!</span> <span class="yarn-meta">#line:0b5d503</span>
<span class="yarn-cmd">&lt;&lt;jump activity_bake_gingerbread&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-bake-gingerbread"></a>

## activity_bake_gingerbread

<div class="yarn-node" data-title="activity_bake_gingerbread">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">tags: activity</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card gingerbread_mold&gt;&gt;</span>
<span class="yarn-line">Match GINGERBREAD parts to their seller.</span> <span class="yarn-meta">#line:0e54683 </span>
<span class="yarn-cmd">&lt;&lt;activity match_ingredients activity_match_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match-done"></a>

## activity_match_done

<div class="yarn-node" data-title="activity_match_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("match_ingredients") &gt; 0&gt;&gt;
<span class="yarn-line">Well done! You matched all the ingredients.</span> <span class="yarn-meta">#line:01648b2</span>
<span class="yarn-cmd">&lt;&lt;card gingerbread&gt;&gt;</span>
<span class="yarn-line">Now I can cook the GINGERBREAD!</span> <span class="yarn-meta">#line:0f0f617 </span>
<span class="yarn-line">Please now go inside the Old Town Hall...</span> <span class="yarn-meta">#line:01c48d3 </span>
<span class="yarn-line">there is a surprise waiting for you!</span> <span class="yarn-meta">#line:0e25ece </span>
<span class="yarn-cmd">&lt;&lt;set $gingerbread_done = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target npc_castle&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Not perfect. Try again!</span> <span class="yarn-meta">#line:007427a </span>
<span class="yarn-cmd">&lt;&lt;jump activity_bake_gingerbread&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer"></a>

## grocer

<div class="yarn-node" data-title="grocer">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ----------------------------------------------</span>
<span class="yarn-header-dim">// GROCER</span>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_grocer") &gt; 0&gt;&gt;
<span class="yarn-line">   You already bought from me!</span> <span class="yarn-meta">#line:already_bought</span>
<span class="yarn-line">   Do you want to play again?</span> <span class="yarn-meta">#line:play_again</span>
<span class="yarn-line">   Yes</span> <span class="yarn-meta">#line:yes</span>
     <span class="yarn-cmd">&lt;&lt;activity hard_money_zloty hard_payment_done&gt;&gt;</span>
<span class="yarn-line">   No</span> <span class="yarn-meta">#line:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Thanks!</span> <span class="yarn-meta">#line:thanks </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Good morning!</span> <span class="yarn-meta">#line:hello </span>
   <span class="yarn-cmd">&lt;&lt;jump grocer_bonjour&gt;&gt;</span>
<span class="yarn-line">Dobranoc!</span> <span class="yarn-meta">#line:0b4db3b </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-bonjour"></a>

## grocer_bonjour

<div class="yarn-node" data-title="grocer_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card person_grocer&gt;&gt;</span>
<span class="yarn-line">Hello! I am a grocer. I sell many kinds of food.</span> <span class="yarn-meta">#line:0ffbfa4</span>
<span class="yarn-line">What do you need?</span> <span class="yarn-meta">#line:0c6a554 </span>
<span class="yarn-cmd">&lt;&lt;jump grocer_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-question"></a>

## grocer_question

<div class="yarn-node" data-title="grocer_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">What do you want to buy?</span> <span class="yarn-meta">#line:what_to_buy </span>
<span class="yarn-line">Fish</span> <span class="yarn-meta">#line:0d6dabd </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Meat</span> <span class="yarn-meta">#line:03eeda4 </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Dress</span> <span class="yarn-meta">#line:097fca2 </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Flour and sugar</span> <span class="yarn-meta">#line:0068f15 </span>
   <span class="yarn-cmd">&lt;&lt;jump grocer_pay_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-pay-activity"></a>

## grocer_pay_activity

<div class="yarn-node" data-title="grocer_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card currency_zloty&gt;&gt;</span>
<span class="yarn-line">Select enough money to pay.</span> <span class="yarn-meta">#line:select_money</span>
<span class="yarn-cmd">&lt;&lt;activity money_grocer grocer_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-payment-done"></a>

## grocer_payment_done

<div class="yarn-node" data-title="grocer_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I put your items in the table. Thank you!</span> <span class="yarn-meta">#line:0567082 </span>
<span class="yarn-line">Goodbye!</span> <span class="yarn-meta">#line:goodbye</span>
-&gt; Thanks! #shadow:thanks
<span class="yarn-line">Have a nice day!</span> <span class="yarn-meta">#line:nice_day</span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Grocer&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-beekeper"></a>

## beekeper

<div class="yarn-node" data-title="beekeper">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ----------------------------------------------</span>
<span class="yarn-header-dim">// BEEKEPER</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: beekeper</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_beekeper") &gt; 0&gt;&gt;
      You already bought from me! #shadow:already_bought
      Do you want to play again? #shadow:play_again
      -&gt; Yes #shadow:yes
     <span class="yarn-cmd">&lt;&lt;activity hard_money_zloty hard_payment_done&gt;&gt;</span>
      -&gt; No #shadow:no
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
-&gt; Thanks! #shadow:thanks
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
-&gt; Good morning! #shadow:hello
   <span class="yarn-cmd">&lt;&lt;jump beekeper_bonjour&gt;&gt;</span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#line:06b0535 </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-beekeper-bonjour"></a>

## beekeper_bonjour

<div class="yarn-node" data-title="beekeper_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: beekeper</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card person_beekeper&gt;&gt;</span>
<span class="yarn-line">Good morning! I'm a beekeeper and I sell honey.</span> <span class="yarn-meta">#line:04b4a87</span>
<span class="yarn-cmd">&lt;&lt;card honey&gt;&gt;</span>
<span class="yarn-line">All of my products come from my beehives!</span> <span class="yarn-meta">#line:0aa9ce7</span>
<span class="yarn-cmd">&lt;&lt;jump beekeper_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-beekeper-question"></a>

## beekeper_question

<div class="yarn-node" data-title="beekeper_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: beekeper</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
What do you want to buy? #shadow:what_to_buy
<span class="yarn-line">Honey</span> <span class="yarn-meta">#line:honey</span>
   <span class="yarn-cmd">&lt;&lt;jump beekeper_pay_activity&gt;&gt;</span>
<span class="yarn-line">Chocolate</span> <span class="yarn-meta">#line:chocolate</span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Bread</span> <span class="yarn-meta">#line:bread</span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Milk</span> <span class="yarn-meta">#line:milk</span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-beekeper-pay-activity"></a>

## beekeper_pay_activity

<div class="yarn-node" data-title="beekeper_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: beekeper</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card currency_zloty&gt;&gt;</span>
Select enough money to pay. #shadow:select_money
<span class="yarn-cmd">&lt;&lt;activity money_beekeper beekeper_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-beekeper-payment-done"></a>

## beekeper_payment_done

<div class="yarn-node" data-title="beekeper_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: beekeper</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
I put your items in the table. Thank you! #shadow:0567082
-&gt; Goodbye! #shadow:goodbye
-&gt; Thanks! #shadow:thanks
-&gt; Have a nice day! #shadow:nice_day
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Beekeper&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger"></a>

## cheesemonger

<div class="yarn-node" data-title="cheesemonger">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ---------------------------------------------- </span>
<span class="yarn-header-dim">// CHEESEMONGER</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_cheesemonger") &gt; 0&gt;&gt;
      You already bought from me! #shadow:already_bought
      Do you want to play again? #shadow:play_again
      -&gt; Yes #shadow:yes
     <span class="yarn-cmd">&lt;&lt;activity hard_money_zloty hard_payment_done&gt;&gt;</span>
      -&gt; No #shadow:no
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
-&gt; Thanks! #shadow:thanks
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
-&gt; Good morning! #shadow:hello
   <span class="yarn-cmd">&lt;&lt;jump cheesemonger_bonjour&gt;&gt;</span>
-&gt; Goodbye! #shadow:goodbye
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-bonjour"></a>

## cheesemonger_bonjour

<div class="yarn-node" data-title="cheesemonger_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hi! I sell cheese and butter. I am a cheesemonger.</span> <span class="yarn-meta">#line:09eb222 </span>
<span class="yarn-line">I use both cow milk and goat milk.</span> <span class="yarn-meta">#line:02f4bc9 </span>
<span class="yarn-cmd">&lt;&lt;card person_cheesemonger&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump cheesemonger_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-question"></a>

## cheesemonger_question

<div class="yarn-node" data-title="cheesemonger_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
What do you want to buy? #shadow:what_to_buy
<span class="yarn-line">Butter</span> <span class="yarn-meta">#line:butter </span>
   <span class="yarn-cmd">&lt;&lt;jump cheesemonger_pay_activity&gt;&gt;</span>
<span class="yarn-line">Oil</span> <span class="yarn-meta">#line:057f694 </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Bread</span> <span class="yarn-meta">#line:087919f </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Tomatoes</span> <span class="yarn-meta">#line:067bfab </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-pay-activity"></a>

## cheesemonger_pay_activity

<div class="yarn-node" data-title="cheesemonger_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card currency_zloty&gt;&gt;</span>
Select enough money to pay. #shadow:select_money
<span class="yarn-cmd">&lt;&lt;activity money_cheesemonger cheesemonger_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-payment-done"></a>

## cheesemonger_payment_done

<div class="yarn-node" data-title="cheesemonger_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
I put your items in the table. Thank you! #shadow:0567082
-&gt; Goodbye! #shadow:goodbye
-&gt; Thanks! #shadow:thanks
-&gt; Have a nice day! #shadow:nice_day
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Cheesemonger&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-eggvendor"></a>

## eggvendor

<div class="yarn-node" data-title="eggvendor">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ----------------------------------------------</span>
<span class="yarn-header-dim">// EGG VENDOR</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: eggvendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_eggvendor") &gt; 0&gt;&gt;
      You already bought from me! #shadow:already_bought
      Do you want to play again? #shadow:play_again
      -&gt; Yes #shadow:yes
     <span class="yarn-cmd">&lt;&lt;activity hard_money_zloty hard_payment_done&gt;&gt;</span>
      -&gt; No #shadow:no
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
-&gt; Thanks! #shadow:thanks
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
-&gt; Good morning! #shadow:hello
   <span class="yarn-cmd">&lt;&lt;jump eggvendor_bonjour&gt;&gt;</span>
-&gt; Goodbye! #shadow:goodbye
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-eggvendor-bonjour"></a>

## eggvendor_bonjour

<div class="yarn-node" data-title="eggvendor_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: eggvendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card person_eggvendor&gt;&gt;</span>
<span class="yarn-line">Hi! I sell EGGS. I am an egg vendor.</span> <span class="yarn-meta">#line:09a9960 </span>
<span class="yarn-cmd">&lt;&lt;jump eggvendor_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-eggvendor-question"></a>

## eggvendor_question

<div class="yarn-node" data-title="eggvendor_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: eggvendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
What do you want to buy? #shadow:what_to_buy
<span class="yarn-line">Eggs</span> <span class="yarn-meta">#line:eggs</span>
   <span class="yarn-cmd">&lt;&lt;jump eggvendor_pay_activity&gt;&gt;</span>
<span class="yarn-line">Oil</span> <span class="yarn-meta">#line:06cc62e </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Bread</span> <span class="yarn-meta">#line:059920e </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-eggvendor-pay-activity"></a>

## eggvendor_pay_activity

<div class="yarn-node" data-title="eggvendor_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: eggvendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card currency_zloty&gt;&gt;</span>
Select enough money to pay. #shadow:select_money
<span class="yarn-cmd">&lt;&lt;activity money_eggvendor eggvendor_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-eggvendor-payment-done"></a>

## eggvendor_payment_done

<div class="yarn-node" data-title="eggvendor_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: eggvendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
I put your items in the table. Thank you! #shadow:0567082
-&gt; Goodbye! #shadow:goodbye
-&gt; Thanks! #shadow:thanks
-&gt; Have a nice day! #shadow:nice_day
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Eggvendor&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spicevendor"></a>

## spicevendor

<div class="yarn-node" data-title="spicevendor">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ----------------------------------------------</span>
<span class="yarn-header-dim">// SPICE VENDOR</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: spicevendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_spicevendor") &gt; 0&gt;&gt;
      You already bought from me! #shadow:already_bought
      Do you want to play again? #shadow:play_again
      -&gt; Yes #shadow:yes
     <span class="yarn-cmd">&lt;&lt;activity hard_money_zloty hard_payment_done&gt;&gt;</span>
      -&gt; No #shadow:no
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
-&gt; Thanks! #shadow:thanks
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
-&gt; Good morning! #shadow:hello
   <span class="yarn-cmd">&lt;&lt;jump spicevendor_bonjour&gt;&gt;</span>
-&gt; Goodbye! #shadow:goodbye
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spicevendor-bonjour"></a>

## spicevendor_bonjour

<div class="yarn-node" data-title="spicevendor_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: spicevendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card person_spicevendor&gt;&gt;</span>
<span class="yarn-line">Hi! I sell spices. I am a spice vendor.</span> <span class="yarn-meta">#line:0f83873 </span>
<span class="yarn-cmd">&lt;&lt;jump spicevendor_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spicevendor-question"></a>

## spicevendor_question

<div class="yarn-node" data-title="spicevendor_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: spicevendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
What do you want to buy? #shadow:what_to_buy
<span class="yarn-line">Cinnamon and ginger</span> <span class="yarn-meta">#line:0fe40e7 </span>
   <span class="yarn-cmd">&lt;&lt;jump spicevendor_pay_activity&gt;&gt;</span>
<span class="yarn-line">Butter</span> <span class="yarn-meta">#line:0fa399a </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Honey</span> <span class="yarn-meta">#line:0cec0d0 </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spicevendor-pay-activity"></a>

## spicevendor_pay_activity

<div class="yarn-node" data-title="spicevendor_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: spicevendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card currency_zloty&gt;&gt;</span>
Select enough money to pay. #shadow:select_money
<span class="yarn-cmd">&lt;&lt;activity money_spicevendor spicevendor_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spicevendor-payment-done"></a>

## spicevendor_payment_done

<div class="yarn-node" data-title="spicevendor_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: spicevendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
I put your items in the table. Thank you! #shadow:0567082
-&gt; Goodbye! #shadow:goodbye
-&gt; Thanks! #shadow:thanks
-&gt; Have a nice day! #shadow:nice_day
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Spicevendor&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-dont-understand"></a>

## talk_dont_understand

<div class="yarn-node" data-title="talk_dont_understand">
<pre class="yarn-code" style="--node-color:orange"><code>
<span class="yarn-header-dim">// ----------------------------------------------</span>
<span class="yarn-header-dim">// GENERIC DETOURS</span>
<span class="yarn-header-dim">tags: detour</span>
<span class="yarn-header-dim">color: orange</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Sorry, I don't understand...</span> <span class="yarn-meta">#line:0f9044b </span>
<span class="yarn-line">What?</span> <span class="yarn-meta">#line:09682b7 </span>
<span class="yarn-line">Huh?</span> <span class="yarn-meta">#line:0c1b3e0 </span>

</code>
</pre>
</div>

<a id="ys-node-talk-dont-sell"></a>

## talk_dont_sell

<div class="yarn-node" data-title="talk_dont_sell">
<pre class="yarn-code" style="--node-color:orange"><code>
<span class="yarn-header-dim">tags: detour</span>
<span class="yarn-header-dim">color: orange</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Sorry, I don't sell that.</span> <span class="yarn-meta">#line:08700b0 </span>

</code>
</pre>
</div>

<a id="ys-node-hard-payment-done"></a>

## hard_payment_done

<div class="yarn-node" data-title="hard_payment_done">
<pre class="yarn-code" style="--node-color:orange"><code>
<span class="yarn-header-dim">tags: detour</span>
<span class="yarn-header-dim">color: orange</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Try talking to the other vendors too.</span> <span class="yarn-meta">#line:06ae965 </span>

</code>
</pre>
</div>

<a id="ys-node-item-flour"></a>

## item_flour

<div class="yarn-node" data-title="item_flour">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// ----------------------------------------------</span>
<span class="yarn-header-dim">// ITEMS TO COLLECT</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_bread&gt;&gt;</span>
<span class="yarn-line">Flour</span> <span class="yarn-meta">#line:08e101e </span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-honey"></a>

## item_honey

<div class="yarn-node" data-title="item_honey">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_honey&gt;&gt;</span>
<span class="yarn-line">Honey</span> <span class="yarn-meta">#line:0817d3c </span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-sugar"></a>

## item_sugar

<div class="yarn-node" data-title="item_sugar">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_sugar&gt;&gt;</span>
<span class="yarn-line">Sugar</span> <span class="yarn-meta">#line:05ad31e </span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-butter"></a>

## item_butter

<div class="yarn-node" data-title="item_butter">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_butter&gt;&gt;</span>
<span class="yarn-line">Butter</span> <span class="yarn-meta">#line:0a8a8cc </span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-egg"></a>

## item_egg

<div class="yarn-node" data-title="item_egg">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_egg&gt;&gt;</span>
<span class="yarn-line">Egg</span> <span class="yarn-meta">#line:00ab8e2 </span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-cinnamon"></a>

## item_cinnamon

<div class="yarn-node" data-title="item_cinnamon">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_cinnamon&gt;&gt;</span>
<span class="yarn-line">Cinnamon</span> <span class="yarn-meta">#line:0f00ddd </span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-ginger"></a>

## item_ginger

<div class="yarn-node" data-title="item_ginger">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_ginger&gt;&gt;</span>
<span class="yarn-line">Ginger</span> <span class="yarn-meta">#line:08049d5 </span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-door-castle"></a>

## door_castle

<div class="yarn-node" data-title="door_castle">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// ----------------------------------------------</span>
<span class="yarn-header-dim">// PIEROGI</span>
<span class="yarn-header-dim">type:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The door is locked.</span> <span class="yarn-meta">#line:042ecfc </span>

</code>
</pre>
</div>

<a id="ys-node-npc-castle"></a>

## npc_castle

<div class="yarn-node" data-title="npc_castle">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hallo. What do you want?</span> <span class="yarn-meta">#line:0f11caf </span>
<span class="yarn-line">I want to enter the Old Town Hall.</span> <span class="yarn-meta">#line:0449db3 </span>
   <span class="yarn-cmd">&lt;&lt;if $gingerbread_done == true&gt;&gt;</span>
      <span class="yarn-cmd">&lt;&lt;trigger open_door_castle&gt;&gt;</span>
      <span class="yarn-cmd">&lt;&lt;area area_castle&gt;&gt;</span>
<span class="yarn-line">      Please, come in! There is someone waiting for you.</span> <span class="yarn-meta">#line:0716b5d </span>
      <span class="yarn-cmd">&lt;&lt;target npc_pierogi&gt;&gt;</span>
      <span class="yarn-cmd">&lt;&lt;SetInteractable door_castle false&gt;&gt;</span>
   <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">      The Old Town Hall is closed to visitors.</span> <span class="yarn-meta">#line:096470a </span>
   <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Just looking around.</span> <span class="yarn-meta">#line:09a9858 </span>
<span class="yarn-line">   Alright, have a nice day.</span> <span class="yarn-meta">#line:09ee9bf </span>

</code>
</pre>
</div>

<a id="ys-node-npc-pierogi"></a>

## npc_pierogi

<div class="yarn-node" data-title="npc_pierogi">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("clean_castle")&gt;&gt;</span>
<span class="yarn-line">    Thank you for cleaning up! Now we can start the Pierogi Festival!</span> <span class="yarn-meta">#line:07e852b </span>
    <span class="yarn-cmd">&lt;&lt;card pierogi&gt;&gt;</span>
<span class="yarn-line">    Have you ever tried PIEROGI?</span> <span class="yarn-meta">#line:0da391b </span>
<span class="yarn-line">    Yes</span> <span class="yarn-meta">#line:08ac4ff </span>
<span class="yarn-line">      It's delicious. Isn't it?</span> <span class="yarn-meta">#line:040912f </span>
<span class="yarn-line">    No</span> <span class="yarn-meta">#line:0cb270e </span>
<span class="yarn-line">      It's our traditional dish made of dough filled with various ingredients.</span> <span class="yarn-meta">#line:09da259 </span>
    <span class="yarn-cmd">&lt;&lt;card_hide&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_tower&gt;&gt;</span>
<span class="yarn-line">    I saw that big blue dog going up the stairs!</span> <span class="yarn-meta">#line:0ee5d9b </span>
<span class="yarn-line">    Go find Antura!</span> <span class="yarn-meta">#line:09877a9 </span>
<span class="yarn-line">    I turned on the elevator for you.</span> <span class="yarn-meta">#line:0f5e348 </span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
   <span class="yarn-cmd">&lt;&lt;SetInteractable tower_lever true&gt;&gt;</span>
   <span class="yarn-cmd">&lt;&lt;task_start climb_the_tower antura_tower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Hi. We want to open the Pierogi Festival</span> <span class="yarn-meta">#line:09c418a </span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_trash&gt;&gt;</span>
<span class="yarn-line">    But that big blue dog made a mess in the hall.</span> <span class="yarn-meta">#line:067cefa </span>
<span class="yarn-line">    Can you help us clean the Hall?</span> <span class="yarn-meta">#line:003385d  </span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;area area_full&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive antura_hall false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start clean_castle task_clean_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-clean-done"></a>

## task_clean_done

<div class="yarn-node" data-title="task_clean_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Well done! Go back to the chef.</span> <span class="yarn-meta">#line:08e9b5f </span>
<span class="yarn-cmd">&lt;&lt;target npc_pierogi&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-antura-tower"></a>

## antura_tower

<div class="yarn-node" data-title="antura_tower">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You found Antura!</span> <span class="yarn-meta">#line:0307b40 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kid"></a>

## spawned_kid

<div class="yarn-node" data-title="spawned_kid">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I love sweet GINGERBREAD!</span> <span class="yarn-meta">#line:087d4b0 </span>
<span class="yarn-line">I like the OLD MARKET!</span> <span class="yarn-meta">#line:0090fb8 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-tourist"></a>

## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The Old HALL looks great.</span> <span class="yarn-meta">#line:041418b </span>
<span class="yarn-line">Today I will taste PIEROGI.</span> <span class="yarn-meta">#line:068a9a7</span>
<span class="yarn-line">Torun is really beautiful!</span> <span class="yarn-meta">#line:08bc7dd </span>
<span class="yarn-line">The market is so lively!</span> <span class="yarn-meta">#line:0d92388 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-buyer"></a>

## spawned_buyer

<div class="yarn-node" data-title="spawned_buyer">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: buyers </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I need to buy some fresh ingredients.</span> <span class="yarn-meta">#line:0baa74d </span>
<span class="yarn-line">The market has the best produce.</span> <span class="yarn-meta">#line:042c6f0 </span>
<span class="yarn-line">Fresh fish is the best!</span> <span class="yarn-meta">#line:01269c6 </span>
<span class="yarn-line">I can't wait to cook a delicious meal!</span> <span class="yarn-meta">#line:0fc5cd3 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-currency"></a>

## spawned_currency

<div class="yarn-node" data-title="spawned_currency">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: tourists </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">One Zloty is 100 groszy.</span> <span class="yarn-meta">#line:0e6c526 #card:currency_zloty</span>
<span class="yarn-line">Zlotys have different sizes for each value.</span> <span class="yarn-meta">#line:021819a #card:currency_zloty</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-jobs"></a>

## spawned_jobs

<div class="yarn-node" data-title="spawned_jobs">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: buyers </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">A baker bakes bread.</span> <span class="yarn-meta">#line:0606dc5 #card:person_baker</span>
<span class="yarn-line">A spice vendor sells spices and herbs.</span> <span class="yarn-meta">#line:0ca5a9b #card:person_spicevendor</span>
<span class="yarn-line">A cheesemonger sells cheese and milk.</span> <span class="yarn-meta">#line:0b5b1c5 #card:person_cheesemonger</span>
<span class="yarn-line">A greengrocer sells fruits and veggies.</span> <span class="yarn-meta">#line:03b9a2e #card:person_greengrocer</span>

</code>
</pre>
</div>


