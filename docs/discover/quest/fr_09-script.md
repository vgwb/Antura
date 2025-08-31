---
title: The Colors of the Marseille Market (fr_09) - Script
hide:
---

# The Colors of the Marseille Market (fr_09) - Script
[Quest Index](./index.md) - Language: english - [french](./fr_09-script.fr.md) - [polish](./fr_09-script.pl.md) - [italian](./fr_09-script.it.md)

<div class="yarn-node"><pre class="yarn-code"><code><span class="yarn-header-dim">// </span>
</code></pre></div>

<div class="yarn-node"><pre class="yarn-code"><code><span class="yarn-header-dim">=</span>
<span class="yarn-header-dim">// FR-09 Cotes Dazur Market - Mont Blanc &amp; Mountains  </span>
<span class="yarn-header-dim">// </span>
</code></pre></div>

<div class="yarn-node"><pre class="yarn-code"><code><span class="yarn-header-dim">=</span>
<span class="yarn-header-dim">//</span>
<span class="yarn-header-dim">// Activities:</span>
<span class="yarn-header-dim">// - DONE collect ingredients (bread, fish, orange, lemon, tomato, cheese, salt, pepper, oil)</span>
<span class="yarn-header-dim">// - order ingredients</span>
<span class="yarn-header-dim">//</span>
<span class="yarn-header-dim">// Words used: bread, fish, crab, orange, lemon, tomato, cheese, salt, pepper, oil</span>
<span class="yarn-header-dim">// </span>
</code></pre></div>

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">=</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $ingredients = 0&gt;&gt;</span>
<span class="yarn-line">Welcome to FR-09: Côte d'Azur! It's a sunny day on the coast. <span class="yarn-meta">#line:078e646 </span></span>

</code></pre></div>

<a id="ys-node-baker-bonjour"></a>
## baker_bonjour

<div class="yarn-node" data-title="baker_bonjour"><pre class="yarn-code"><code><span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset shop_baker&gt;&gt;</span>
<span class="yarn-line">Bonjour! I sell fresh bread. I am a baker. <span class="yarn-meta">#line:0c6f41f </span></span>
<span class="yarn-line">Every day I wake up early to bake. <span class="yarn-meta">#line:0f6e48a </span></span>
<span class="yarn-cmd">&lt;&lt;card person_baker zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump baker_question&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-shop-baker"></a>
## shop_baker

<div class="yarn-node" data-title="shop_baker"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">tags: actor=BAKER</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour! <span class="yarn-meta">#line:0cd2ee2 </span></span>
<span class="yarn-line">-&gt; Bonjour!: <span class="yarn-meta">#line:09460ce </span></span>
    <span class="yarn-cmd">&lt;&lt;jump baker_bonjour&gt;&gt;</span>
<span class="yarn-line">-&gt; Merci!: <span class="yarn-meta">#line:0bf3f32 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump baker_notunderstand&gt;&gt;</span>
<span class="yarn-line">-&gt; Banana!: <span class="yarn-meta">#line:0ebf8b2 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump baker_notunderstand&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-baker-question"></a>
## baker_question

<div class="yarn-node" data-title="baker_question"><pre class="yarn-code"><code><span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">tags: actor=BAKER, type=Choice</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">What do you want to buy? <span class="yarn-meta">#line:00279c8 </span></span>
<span class="yarn-line">-&gt; "Bread": <span class="yarn-meta">#line:00eab87 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump baker_pay_activity&gt;&gt;</span>
<span class="yarn-line">-&gt; "Fish and Crab": <span class="yarn-meta">#line:08177a5 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump baker_dontsell&gt;&gt;</span>
<span class="yarn-line">-&gt; "Tomatoes, Oranges, and Lemons": <span class="yarn-meta">#line:0a8294a </span></span>
    <span class="yarn-cmd">&lt;&lt;jump baker_dontsell&gt;&gt;</span>
<span class="yarn-line">-&gt; "Salt, Pepper, and Oil": <span class="yarn-meta">#line:0babba5 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump baker_dontsell&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-baker-pay-activity"></a>
## baker_pay_activity

<div class="yarn-node" data-title="baker_pay_activity"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_BAKER baker_payment_done&gt;&gt;</span>
<span class="yarn-line">MAN: Select enough money to pay. <span class="yarn-meta">#line:0bbf963 </span></span>

</code></pre></div>

<a id="ys-node-baker-payment-done"></a>
## baker_payment_done

<div class="yarn-node" data-title="baker_payment_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">tags: actor=MAN, no_translate</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $ingredients = $ingredients + 1&gt;&gt;</span>
<span class="yarn-line">-&gt; Merci! <span class="yarn-meta">#line:00da30a </span></span>
<span class="yarn-line">-&gt; Au revoir! <span class="yarn-meta">#line:00cbd60 </span></span>
<span class="yarn-line">-&gt; Bonne journée! <span class="yarn-meta">#line:00cd1cf </span></span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Baker&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-baker-dontsell"></a>
## baker_dontsell

<div class="yarn-node" data-title="baker_dontsell"><pre class="yarn-code"><code><span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">MAN: Sorry, I don't sell that. <span class="yarn-meta">#line:0875143 </span></span>

</code></pre></div>

<a id="ys-node-fisher-payment-done"></a>
## fisher_payment_done

<div class="yarn-node" data-title="fisher_payment_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">tags: actor=OLD_MAN, do_not_translate</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">-&gt; "Au revoir!": <span class="yarn-meta">#line:02e64ff </span></span>
<span class="yarn-line">-&gt; "Merci!": <span class="yarn-meta">#line:02c23e5 </span></span>
<span class="yarn-line">-&gt; "Bonne journée!": <span class="yarn-meta">#line:080d945 </span></span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Fisher&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-fisher-dontsell"></a>
## fisher_dontsell

<div class="yarn-node" data-title="fisher_dontsell"><pre class="yarn-code"><code><span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">tags: actor=OLD_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Sorry, I don't sell that. <span class="yarn-meta">#line:01ea288 </span></span>

</code></pre></div>

<a id="ys-node-fisher-question"></a>
## fisher_question

<div class="yarn-node" data-title="fisher_question"><pre class="yarn-code"><code><span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">tags: actor=OLD_MAN, type=Choice</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">What do you want to buy? <span class="yarn-meta">#line:04deddc </span></span>
<span class="yarn-line">-&gt; "Fish &amp; Crab": <span class="yarn-meta">#line:0e562df </span></span>
    <span class="yarn-cmd">&lt;&lt;jump fisher_pay_activity&gt;&gt;</span>
<span class="yarn-line">-&gt; "Tomatoes, Oranges, and Lemons": <span class="yarn-meta">#line:085463e </span></span>
    <span class="yarn-cmd">&lt;&lt;jump fisher_dontsell&gt;&gt;</span>
<span class="yarn-line">-&gt; "Bread": <span class="yarn-meta">#line:0604902 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump fisher_dontsell&gt;&gt;</span>
<span class="yarn-line">-&gt; "Milk": <span class="yarn-meta">#line:0c5f144 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump fisher_dontsell&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-shop-fisher"></a>
## shop_fisher

<div class="yarn-node" data-title="shop_fisher"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">tags: actor=OLD_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">-&gt; "Lycée!": <span class="yarn-meta">#line:0d65316 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">-&gt; "Bonjour!": <span class="yarn-meta">#line:089f618 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump fisher_bonjour&gt;&gt;</span>
<span class="yarn-line">-&gt; "Au revoir!": <span class="yarn-meta">#line:06b0535 </span></span>
    <span class="yarn-cmd">&lt;&lt;detour talk_dont_understand&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-fisher-bonjour"></a>
## fisher_bonjour

<div class="yarn-node" data-title="fisher_bonjour"><pre class="yarn-code"><code><span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">tags: actor=OLD_MAN</span>
<span class="yarn-header-dim">actor: OLD_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Good morning! I sell fish and crab. I'm a fishmonger. <span class="yarn-meta">#line:04b4a87 </span></span>
<span class="yarn-line">All of my items come from the sea! <span class="yarn-meta">#line:0aa9ce7 </span></span>
<span class="yarn-cmd">&lt;&lt;card  person_fishmonger zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump fisher_question&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-shop-cheesemonger"></a>
## shop_cheesemonger

<div class="yarn-node" data-title="shop_cheesemonger"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">tags: actor=WOMAN, do_not_translate</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">-&gt; "Merci!": <span class="yarn-meta">#line:0693ba6 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump cheesemonger_notunderstand&gt;&gt;</span>
<span class="yarn-line">-&gt; "Bonjour!": <span class="yarn-meta">#line:022bf02 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump cheesemonger_bonjour&gt;&gt;</span>
<span class="yarn-line">-&gt; "Chat!": <span class="yarn-meta">#line:0b56a4d </span></span>
    <span class="yarn-cmd">&lt;&lt;jump cheesemonger_notunderstand&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-cheesemonger-question"></a>
## cheesemonger_question

<div class="yarn-node" data-title="cheesemonger_question"><pre class="yarn-code"><code><span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">tags: actor=WOMAN, type=Choice</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">What do you want to buy? <span class="yarn-meta">#line:03009de </span></span>
<span class="yarn-line">-&gt; "Milk": <span class="yarn-meta">#line:0aa7def </span></span>
    <span class="yarn-cmd">&lt;&lt;jump cheesemonger_pay_activity&gt;&gt;</span>
<span class="yarn-line">-&gt; "Salt, Pepper, and Oil": <span class="yarn-meta">#line:057f694 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump cheesemonger_dontsell&gt;&gt;</span>
<span class="yarn-line">-&gt; "Bread": <span class="yarn-meta">#line:087919f </span></span>
    <span class="yarn-cmd">&lt;&lt;jump cheesemonger_dontsell&gt;&gt;</span>
<span class="yarn-line">-&gt; "Tomatoes, Oranges, and Lemons": <span class="yarn-meta">#line:067bfab </span></span>
    <span class="yarn-cmd">&lt;&lt;jump cheesemonger_dontsell&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-cheesemonger-payment-done"></a>
## cheesemonger_payment_done

<div class="yarn-node" data-title="cheesemonger_payment_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group:cheesemonger</span>
<span class="yarn-header-dim">tags: actor=WOMAN, do_not_translate</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">-&gt; "Bonne journée!": <span class="yarn-meta">#line:0dd3ac3 </span></span>
<span class="yarn-line">-&gt; "Au revoir!": <span class="yarn-meta">#line:02a1238 </span></span>
<span class="yarn-line">-&gt; "Merci!": <span class="yarn-meta">#line:0273de1 </span></span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Cheesemonger&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-cheesemonger-bonjour"></a>
## cheesemonger_bonjour

<div class="yarn-node" data-title="cheesemonger_bonjour"><pre class="yarn-code"><code><span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hi! I sell cheese and milk. I am a cheesemonger. <span class="yarn-meta">#line:09eb222 </span></span>
<span class="yarn-line">I use both cow milk and goat milk. <span class="yarn-meta">#line:02f4bc9 </span></span>
<span class="yarn-cmd">&lt;&lt;card  person_cheesemonger zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump cheesemonger_question&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-cheesemonger-dontsell"></a>
## cheesemonger_dontsell

<div class="yarn-node" data-title="cheesemonger_dontsell"><pre class="yarn-code"><code><span class="yarn-header-dim">group:cheesemonger</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Sorry, I don't sell that. <span class="yarn-meta">#line:058fc6d </span></span>

</code></pre></div>

<a id="ys-node-greengrocer-dontsell"></a>
## greengrocer_dontsell

<div class="yarn-node" data-title="greengrocer_dontsell"><pre class="yarn-code"><code><span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">MAN: Sorry, I don't sell that. <span class="yarn-meta">#line:03b0024 </span></span>

</code></pre></div>

<a id="ys-node-greengrocer-payment-activity"></a>
## greengrocer_payment_activity

<div class="yarn-node" data-title="greengrocer_payment_activity"><pre class="yarn-code"><code><span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">tags: actor=WOMAN, do_not_translate</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">-&gt; "Bonne journée!": <span class="yarn-meta">#line:0ff9361 </span></span>
<span class="yarn-line">-&gt; "Merci!": <span class="yarn-meta">#line:0741be3 </span></span>
<span class="yarn-line">-&gt; "Au revoir!": <span class="yarn-meta">#line:023f352 </span></span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Greengrocer&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-greengrocer-question"></a>
## greengrocer_question

<div class="yarn-node" data-title="greengrocer_question"><pre class="yarn-code"><code><span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">What do you want to buy? <span class="yarn-meta">#line:042eb5a </span></span>
<span class="yarn-line">-&gt; "Fish and crab": <span class="yarn-meta">#line:0fead1d </span></span>
    <span class="yarn-cmd">&lt;&lt;jump greengrocer_dontsell&gt;&gt;</span>
<span class="yarn-line">-&gt; "Bread": <span class="yarn-meta">#line:0879f58 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump greengrocer_dontsell&gt;&gt;</span>
<span class="yarn-line">-&gt; "Tomatoes, Oranges, and Lemons": <span class="yarn-meta">#line:015a2b4 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump greengrocer_pay_activity&gt;&gt;</span>
<span class="yarn-line">-&gt; "Milk": <span class="yarn-meta">#line:0fd3f3a </span></span>
    <span class="yarn-cmd">&lt;&lt;jump greengrocer_dontsell&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-shop-greengrocer"></a>
## shop_greengrocer

<div class="yarn-node" data-title="shop_greengrocer"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">-&gt; "Merci!": <span class="yarn-meta">#line:0a43c28 </span></span>
    <span class="yarn-cmd">&lt;&lt;detour talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">-&gt; "Train!": <span class="yarn-meta">#line:02af86a </span></span>
    <span class="yarn-cmd">&lt;&lt;detour talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">-&gt; "Bonjour!": <span class="yarn-meta">#line:0039ce8 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump greengrocer_bonjour&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-greengrocer-bonjour"></a>
## greengrocer_bonjour

<div class="yarn-node" data-title="greengrocer_bonjour"><pre class="yarn-code"><code><span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hello! I sell fruits and vegetables. I'm a greengrocer. <span class="yarn-meta">#line:041ade1 </span></span>
<span class="yarn-line">My items are always fresh! <span class="yarn-meta">#line:0969b87 </span></span>
<span class="yarn-cmd">&lt;&lt;card  person_greengrocer zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump greengrocer_question&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-grocer-payment-done"></a>
## grocer_payment_done

<div class="yarn-node" data-title="grocer_payment_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN, do_not_translate</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">-&gt; "Au revoir!": <span class="yarn-meta">#line:0ce6f8a </span></span>
<span class="yarn-line">-&gt; "Merci!": <span class="yarn-meta">#line:0e8ec1b </span></span>
<span class="yarn-line">-&gt; "Bonne journée!": <span class="yarn-meta">#line:062029a </span></span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Grocer&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-grocer-dontsell"></a>
## grocer_dontsell

<div class="yarn-node" data-title="grocer_dontsell"><pre class="yarn-code"><code><span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">action: OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Sorry, I don't sell that. <span class="yarn-meta">#line:0977493 </span></span>

</code></pre></div>

<a id="ys-node-grocer-question"></a>
## grocer_question

<div class="yarn-node" data-title="grocer_question"><pre class="yarn-code"><code><span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN, type=Choice</span>
<span class="yarn-header-dim">actor: OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">What do you want to buy? <span class="yarn-meta">#line:0c36100 </span></span>
<span class="yarn-line">-&gt; "Tomatoes, Oranges, and Lemons": <span class="yarn-meta">#line:0d6dabd </span></span>
    <span class="yarn-cmd">&lt;&lt;jump grocer_dontsell&gt;&gt;</span>
<span class="yarn-line">-&gt; "Bread": <span class="yarn-meta">#line:03eeda4 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump grocer_dontsell&gt;&gt;</span>
<span class="yarn-line">-&gt; "Milk": <span class="yarn-meta">#line:097fca2 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump grocer_dontsell&gt;&gt;</span>
<span class="yarn-line">-&gt; "Salt, Pepper, and Oil": <span class="yarn-meta">#line:0068f15 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump grocer_pay_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-shop-grocer"></a>
## shop_grocer

<div class="yarn-node" data-title="shop_grocer"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">-&gt; "Merci!": <span class="yarn-meta">#line:0dd992c </span></span>
    <span class="yarn-cmd">&lt;&lt;jump grocer_notunderstand&gt;&gt;</span>
<span class="yarn-line">-&gt; "Bonjour!": <span class="yarn-meta">#line:0623f71 </span></span>
    <span class="yarn-cmd">&lt;&lt;jump grocer_bonjour&gt;&gt;</span>
<span class="yarn-line">-&gt; "Livre!": <span class="yarn-meta">#line:0b4db3b </span></span>
    <span class="yarn-cmd">&lt;&lt;jump grocer_notunderstand&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-grocer-bonjour"></a>
## grocer_bonjour

<div class="yarn-node" data-title="grocer_bonjour"><pre class="yarn-code"><code><span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">actor: OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hello! I sell spices and pantry goods. I am a grocer. <span class="yarn-meta">#line:0ffbfa4 </span></span>
<span class="yarn-line">You can use my items for many recipes. <span class="yarn-meta">#line:0c6a554 </span></span>
<span class="yarn-cmd">&lt;&lt;card  person_grocer zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump grocer_question&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-pirate"></a>
## pirate

<div class="yarn-node" data-title="pirate"><pre class="yarn-code"><code><span class="yarn-header-dim">group: pirates</span>
<span class="yarn-header-dim">tags: actor=CRAZY_MAN</span>
<span class="yarn-header-dim">actor: CRAZY_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ahoy! Welcome aboard. <span class="yarn-meta">#line:04ee922 </span></span>
<span class="yarn-line">We come from Saint-Malo and sailed the seas to get here. <span class="yarn-meta">#line:056c70d </span></span>
<span class="yarn-line">People call us pirates, but we were corsairs—privateers. <span class="yarn-meta">#line:0f764a6 </span></span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_JIGSAW activity_pirate_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-pirate-done"></a>
## activity_pirate_done

<div class="yarn-node" data-title="activity_pirate_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: pirates</span>
<span class="yarn-header-dim">tags: actor=CRAZY_MAN</span>
<span class="yarn-header-dim">actor: CRAZY_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">We worked for the King of France. <span class="yarn-meta">#line:0af3bba </span></span>
<span class="yarn-line">In the past, we took items from the king's enemies, but those days are gone. <span class="yarn-meta">#line:0b61715 </span></span>
<span class="yarn-line">Please pay for what you need so the shops can stay open! <span class="yarn-meta">#line:02c49d0 </span></span>

</code></pre></div>

<a id="ys-node-chef"></a>
## chef

<div class="yarn-node" data-title="chef"><pre class="yarn-code" style="--node-color:blue"><code><span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $ingredients == 5&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump chef_ingredients_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump chef_welcome&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-chef-welcome"></a>
## chef_welcome

<div class="yarn-node" data-title="chef_welcome"><pre class="yarn-code"><code><span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour! Welcome to Marseille, on the Mediterranean Sea! <span class="yarn-meta">#line:02548dd </span></span>
<span class="yarn-line">I want to make a special dish for you, a bouillabaisse! <span class="yarn-meta">#line:0c65de3 </span></span>
<span class="yarn-cmd">&lt;&lt;card food_bouillabaisse_fr zoom&gt;&gt;</span>
<span class="yarn-line">But I need some ingredients. <span class="yarn-meta">#line:0623733 </span></span>
<span class="yarn-cmd">&lt;&lt;jump task_ingredients&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-ingredients"></a>
## task_ingredients

<div class="yarn-node" data-title="task_ingredients"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Please go to the market and buy them for me. <span class="yarn-meta">#line:06602c6 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start COLLECT_THE_INGREDIENTS&gt;&gt;</span>
<span class="yarn-line">Remember your manners! <span class="yarn-meta">#line:02819db </span></span>
<span class="yarn-line">Say "Bonjour" to greet someone, <span class="yarn-meta">#line:04c9d69 </span></span>
<span class="yarn-line">and "Merci" to thank them. <span class="yarn-meta">#line:022fd8f </span></span>

</code></pre></div>

<a id="ys-node-task-ingredients-done"></a>
## task_ingredients_done

<div class="yarn-node" data-title="task_ingredients_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You got all the ingredients! Go back to the chef. <span class="yarn-meta">#line:05398f2 </span></span>

</code></pre></div>

<a id="ys-node-chef-ingredients-done"></a>
## chef_ingredients_done

<div class="yarn-node" data-title="chef_ingredients_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Magnifique! You have everything. <span class="yarn-meta">#line:0257fc7 </span></span>
<span class="yarn-line">You were very polite. <span class="yarn-meta">#line:0112e25 </span></span>
<span class="yarn-line">Now, let's prepare our feast! <span class="yarn-meta">#line:0051174 </span></span>
<span class="yarn-cmd">&lt;&lt;jump activity_match_ingredients&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-match-ingredients"></a>
## activity_match_ingredients

<div class="yarn-node" data-title="activity_match_ingredients"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">tags: activity</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_ORDER activity_match_done&gt;&gt;</span>
<span class="yarn-line">Match each item to the right seller. <span class="yarn-meta">#line:0a6e106 </span></span>

</code></pre></div>

<a id="ys-node-activity-match-done"></a>
## activity_match_done

<div class="yarn-node" data-title="activity_match_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Well done! You matched all the items. <span class="yarn-meta">#line:01648b2 </span></span>
<span class="yarn-line">Now, let's cook the bouillabaisse! <span class="yarn-meta">#line:0f0f617 </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end 3&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-fisher-pay-activity"></a>
## fisher_pay_activity

<div class="yarn-node" data-title="fisher_pay_activity"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">tags: actor=OLD_MAN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_FISHER fisher_payment_done&gt;&gt;</span>
<span class="yarn-line">Select enough money to pay. <span class="yarn-meta">#line:0995020 </span></span>

</code></pre></div>

<a id="ys-node-cheesemonger-pay-activity"></a>
## cheesemonger_pay_activity

<div class="yarn-node" data-title="cheesemonger_pay_activity"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group:cheesemonger</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_CHEESEMONGER cheesemonger_payment_activity&gt;&gt;</span>
<span class="yarn-line">Select enough money to pay. <span class="yarn-meta">#line:0f44ea7 </span></span>

</code></pre></div>

<a id="ys-node-grocer-pay-activity"></a>
## grocer_pay_activity

<div class="yarn-node" data-title="grocer_pay_activity"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_GROCER grocer_payment_done&gt;&gt;</span>
<span class="yarn-line">Select enough money to pay. <span class="yarn-meta">#line:0c80f9e </span></span>

</code></pre></div>

<a id="ys-node-greengrocer-pay-activity"></a>
## greengrocer_pay_activity

<div class="yarn-node" data-title="greengrocer_pay_activity"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_GREENGROCER greengrocer_payment_activity&gt;&gt;</span>
<span class="yarn-line">Select enough money to pay. <span class="yarn-meta">#line:08fc94e </span></span>

</code></pre></div>

<a id="ys-node-baker-notunderstand"></a>
## baker_notunderstand

<div class="yarn-node" data-title="baker_notunderstand"><pre class="yarn-code"><code><span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Sorry, I don't think I understand... <span class="yarn-meta">#line:0db5121 </span></span>

</code></pre></div>

<a id="ys-node-cheesemonger-notunderstand"></a>
## cheesemonger_notunderstand

<div class="yarn-node" data-title="cheesemonger_notunderstand"><pre class="yarn-code"><code><span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Sorry, I don't think I understand... <span class="yarn-meta">#line:01a0ec9 </span></span>

</code></pre></div>

<a id="ys-node-grocer-notunderstand"></a>
## grocer_notunderstand

<div class="yarn-node" data-title="grocer_notunderstand"><pre class="yarn-code"><code><span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Sorry, I don't think I understand... <span class="yarn-meta">#line:0a30381 </span></span>

</code></pre></div>

<a id="ys-node-talk-dont-understand"></a>
## talk_dont_understand

<div class="yarn-node" data-title="talk_dont_understand"><pre class="yarn-code" style="--node-color:orange"><code><span class="yarn-header-dim">tags: detour</span>
<span class="yarn-header-dim">color: orange</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">=&gt; Sorry, I don't think I understand... <span class="yarn-meta">#line:0f9044b </span></span>
<span class="yarn-line">=&gt; What?? <span class="yarn-meta">#line:09682b7 </span></span>
<span class="yarn-line">=&gt; Jen ne comprends pas... <span class="yarn-meta">#line:0c1b3e0 </span></span>

</code></pre></div>

<a id="ys-node-item-bread"></a>
## item_bread

<div class="yarn-node" data-title="item_bread"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">TUTOR: Bread <span class="yarn-meta">#line:08e101e </span></span>

</code></pre></div>

<a id="ys-node-item-fish"></a>
## item_fish

<div class="yarn-node" data-title="item_fish"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">TUTOR: Fish <span class="yarn-meta">#line:0feed79 </span></span>

</code></pre></div>

<a id="ys-node-item-orange"></a>
## item_orange

<div class="yarn-node" data-title="item_orange"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">TUTOR: Orange <span class="yarn-meta">#line:0c0fa04 </span></span>

</code></pre></div>

<a id="ys-node-item-lemon"></a>
## item_lemon

<div class="yarn-node" data-title="item_lemon"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">TUTOR: Lemon <span class="yarn-meta">#line:0c6b991 </span></span>

</code></pre></div>

<a id="ys-node-item-tomato"></a>
## item_tomato

<div class="yarn-node" data-title="item_tomato"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">TUTOR: Tomato <span class="yarn-meta">#line:0a6782d </span></span>

</code></pre></div>

<a id="ys-node-item-cheese"></a>
## item_cheese

<div class="yarn-node" data-title="item_cheese"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">TUTOR: Cheese <span class="yarn-meta">#line:0acd781 </span></span>

</code></pre></div>

<a id="ys-node-item-salt"></a>
## item_salt

<div class="yarn-node" data-title="item_salt"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">TUTOR: Salt <span class="yarn-meta">#line:07bbcb0 </span></span>

</code></pre></div>

<a id="ys-node-item-pepper"></a>
## item_pepper

<div class="yarn-node" data-title="item_pepper"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">TUTOR: Pepper <span class="yarn-meta">#line:04d85d3 </span></span>

</code></pre></div>

<a id="ys-node-item-oil"></a>
## item_oil

<div class="yarn-node" data-title="item_oil"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">TUTOR: Oil <span class="yarn-meta">#line:0156410 </span></span>

</code></pre></div>


