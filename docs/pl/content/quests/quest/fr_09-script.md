---
title: Jedzenie i targ (fr_09) - Script
hide:
---

# Jedzenie i targ (fr_09) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-09-the-colors-of-the-marseille-market/28/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1243903291#gid=1243903291)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_09%20Food%20&%20Market/FR_09%20Food%20&%20Market%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_09 | Food &amp; Market (Marseille)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $ingredients = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $baker_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $cheesemonger_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $fishmonger_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $greengrocer_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $grocer_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $pirate_completed = false&gt;&gt;</span>
<span class="yarn-line">Witamy na Lazurowym Wybrzeżu! Na wybrzeżu jest słoneczny dzień.</span> <span class="yarn-meta">#line:078e646 </span>

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
<span class="yarn-line">Gra ukończona! Gratulacje!</span> <span class="yarn-meta">#line:0f5c958 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_proposal&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-proposal"></a>

## quest_proposal

<div class="yarn-node" data-title="quest_proposal">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jakie jest twoje ulubione jedzenie?</span> <span class="yarn-meta">#line:01f78ed </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-bonjour"></a>

## baker_bonjour

<div class="yarn-node" data-title="baker_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dzień dobry! Sprzedaję świeży chleb. Jestem piekarzem.</span> <span class="yarn-meta">#line:0c6f41f </span>
<span class="yarn-line">Codziennie wstaję wcześnie rano, żeby piec.</span> <span class="yarn-meta">#line:0f6e48a </span>
<span class="yarn-cmd">&lt;&lt;card person_baker zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump baker_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-shop-baker"></a>

## shop_baker

<div class="yarn-node" data-title="shop_baker">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">tags: actor=MAN_BIG, noRepeatLastLine</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $baker_completed&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: You already bought bread from me!]</span> <span class="yarn-meta">#line:023d379 </span>
<span class="yarn-line">[MISSING TRANSLATION: Try talking to the other vendors.]</span> <span class="yarn-meta">#line:0b6176e </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Dzień dobry!</span> <span class="yarn-meta">#line:09460ce </span>
    <span class="yarn-cmd">&lt;&lt;jump baker_bonjour&gt;&gt;</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:0bf3f32 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Banan!</span> <span class="yarn-meta">#line:0ebf8b2 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-question"></a>

## baker_question

<div class="yarn-node" data-title="baker_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">tags: actor=MAN_BIG, type=Choice</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Co chcesz kupić?</span> <span class="yarn-meta">#line:00279c8 </span>
<span class="yarn-line">Chleb</span> <span class="yarn-meta">#line:00eab87 </span>
    <span class="yarn-cmd">&lt;&lt;jump baker_pay_activity&gt;&gt;</span>
<span class="yarn-line">Ryby i kraby</span> <span class="yarn-meta">#line:08177a5 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pomidory, pomarańcze i cytryny</span> <span class="yarn-meta">#line:0a8294a </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Sól, pieprz i olej</span> <span class="yarn-meta">#line:0babba5 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-pay-activity"></a>

## baker_pay_activity

<div class="yarn-node" data-title="baker_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wybierz kwotę wystarczającą do zapłaty.</span> <span class="yarn-meta">#line:0bbf963 </span>
<span class="yarn-cmd">&lt;&lt;activity money_baker baker_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-payment-done"></a>

## baker_payment_done

<div class="yarn-node" data-title="baker_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">tags: actor=MAN, no_translate, noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:00da30a </span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#line:00cbd60 </span>
<span class="yarn-line">Bonne journée!</span> <span class="yarn-meta">#line:00cd1cf </span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Baker&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $baker_completed = true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-dontsell"></a>

## baker_dontsell

<div class="yarn-node" data-title="baker_dontsell">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Przykro mi, ale tego nie sprzedaję.</span> <span class="yarn-meta">#line:0875143 </span>

</code>
</pre>
</div>

<a id="ys-node-fisher-payment-done"></a>

## fisher_payment_done

<div class="yarn-node" data-title="fisher_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">tags: actor=MAN_OLD, do_not_translate, noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#line:02e64ff </span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:02c23e5 </span>
<span class="yarn-line">Bonne journée!</span> <span class="yarn-meta">#line:080d945 </span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Fisherman&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $fishmonger_completed = true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-fisher-dontsell"></a>

## fisher_dontsell

<div class="yarn-node" data-title="fisher_dontsell">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">tags: actor=MAN_OLD</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Przykro mi, ale tego nie sprzedaję.</span> <span class="yarn-meta">#line:01ea288 </span>

</code>
</pre>
</div>

<a id="ys-node-fisher-question"></a>

## fisher_question

<div class="yarn-node" data-title="fisher_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">tags: actor=MAN_OLD</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Co chcesz kupić?</span> <span class="yarn-meta">#line:04deddc </span>
<span class="yarn-line">Ryby i kraby</span> <span class="yarn-meta">#line:0e562df </span>
    <span class="yarn-cmd">&lt;&lt;jump fisher_pay_activity&gt;&gt;</span>
<span class="yarn-line">Pomidory, pomarańcze i cytryny</span> <span class="yarn-meta">#line:085463e </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Chleb</span> <span class="yarn-meta">#line:0604902 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Mleko</span> <span class="yarn-meta">#line:0c5f144 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-shop-fisher"></a>

## shop_fisher

<div class="yarn-node" data-title="shop_fisher">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">tags: actor=MAN_OLD, noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $fishmonger_completed&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: You already bought fish from me!]</span> <span class="yarn-meta">#line:0b3f247 </span>
<span class="yarn-line">[MISSING TRANSLATION: Try the next shop.]</span> <span class="yarn-meta">#line:0ed0200 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Liceum!</span> <span class="yarn-meta">#line:0d65316 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Dzień dobry!</span> <span class="yarn-meta">#line:089f618 </span>
    <span class="yarn-cmd">&lt;&lt;jump fisher_bonjour&gt;&gt;</span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#line:06b0535 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-fisher-bonjour"></a>

## fisher_bonjour

<div class="yarn-node" data-title="fisher_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">tags: actor=MAN_OLD</span>
<span class="yarn-header-dim">actor: OLD_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dzień dobry! Sprzedaję ryby i kraby. Jestem handlarzem ryb.</span> <span class="yarn-meta">#line:04b4a87 </span>
<span class="yarn-line">Wszystkie moje rzeczy pochodzą z morza!</span> <span class="yarn-meta">#line:0aa9ce7 </span>
<span class="yarn-cmd">&lt;&lt;card  person_fishmonger zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump fisher_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-shop-cheesemonger"></a>

## shop_cheesemonger

<div class="yarn-node" data-title="shop_cheesemonger">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">tags: actor=WOMAN, do_not_translate, noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $cheesemonger_completed&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: You already bought milk from me!]</span> <span class="yarn-meta">#line:082465a </span>
<span class="yarn-line">[MISSING TRANSLATION: Try talking to the others.]</span> <span class="yarn-meta">#line:026a894 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:0693ba6 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Dzień dobry!</span> <span class="yarn-meta">#line:022bf02 </span>
    <span class="yarn-cmd">&lt;&lt;jump cheesemonger_bonjour&gt;&gt;</span>
<span class="yarn-line">Pogawędzić!</span> <span class="yarn-meta">#line:0b56a4d </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-question"></a>

## cheesemonger_question

<div class="yarn-node" data-title="cheesemonger_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Co chcesz kupić?</span> <span class="yarn-meta">#line:03009de </span>
<span class="yarn-line">Mleko</span> <span class="yarn-meta">#line:0aa7def </span>
    <span class="yarn-cmd">&lt;&lt;jump cheesemonger_pay_activity&gt;&gt;</span>
<span class="yarn-line">Sól, pieprz i olej</span> <span class="yarn-meta">#line:057f694 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Chleb</span> <span class="yarn-meta">#line:087919f </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pomidory, pomarańcze i cytryny</span> <span class="yarn-meta">#line:067bfab </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-payment-done"></a>

## cheesemonger_payment_done

<div class="yarn-node" data-title="cheesemonger_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group:cheesemonger</span>
<span class="yarn-header-dim">tags: actor=WOMAN, do_not_translate, noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonne journée!</span> <span class="yarn-meta">#line:0dd3ac3 </span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#line:02a1238 </span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:0273de1 </span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Cheesemonger&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $cheesemonger_completed = true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-bonjour"></a>

## cheesemonger_bonjour

<div class="yarn-node" data-title="cheesemonger_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cześć! Sprzedaję ser i mleko. Jestem sprzedawcą serów.</span> <span class="yarn-meta">#line:09eb222 </span>
<span class="yarn-line">Używam zarówno mleka krowiego, jak i koziego.</span> <span class="yarn-meta">#line:02f4bc9 </span>
<span class="yarn-cmd">&lt;&lt;card  person_cheesemonger zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump cheesemonger_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-dontsell"></a>

## cheesemonger_dontsell

<div class="yarn-node" data-title="cheesemonger_dontsell">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group:cheesemonger</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Przykro mi, ale tego nie sprzedaję.</span> <span class="yarn-meta">#line:058fc6d </span>

</code>
</pre>
</div>

<a id="ys-node-greengrocer-dontsell"></a>

## greengrocer_dontsell

<div class="yarn-node" data-title="greengrocer_dontsell">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">MĘŻCZYZNA: Przepraszam, nie sprzedaję tego.</span> <span class="yarn-meta">#line:03b0024 </span>

</code>
</pre>
</div>

<a id="ys-node-greengrocer-payment-activity"></a>

## greengrocer_payment_activity

<div class="yarn-node" data-title="greengrocer_payment_activity">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">tags: actor=WOMAN, do_not_translate, noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonne journée!</span> <span class="yarn-meta">#line:0ff9361 </span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:0741be3 </span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#line:023f352 </span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Greengrocer&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $greengrocer_completed = true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-greengrocer-question"></a>

## greengrocer_question

<div class="yarn-node" data-title="greengrocer_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Co chcesz kupić?</span> <span class="yarn-meta">#line:042eb5a </span>
<span class="yarn-line">Ryby i kraby</span> <span class="yarn-meta">#line:0fead1d </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Chleb</span> <span class="yarn-meta">#line:0879f58 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pomidory, pomarańcze i cytryny</span> <span class="yarn-meta">#line:015a2b4 </span>
    <span class="yarn-cmd">&lt;&lt;jump greengrocer_pay_activity&gt;&gt;</span>
<span class="yarn-line">Mleko</span> <span class="yarn-meta">#line:0fd3f3a </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-shop-greengrocer"></a>

## shop_greengrocer

<div class="yarn-node" data-title="shop_greengrocer">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">tags: actor=WOMAN, noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $greengrocer_completed&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: You already bought some fruit from me!]</span> <span class="yarn-meta">#line:0e767fe </span>
<span class="yarn-line">[MISSING TRANSLATION: Try talking to another vendor.]</span> <span class="yarn-meta">#line:00a927b </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:0a43c28 </span>
    <span class="yarn-cmd">&lt;&lt;detour talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Pociąg!</span> <span class="yarn-meta">#line:02af86a </span>
    <span class="yarn-cmd">&lt;&lt;detour talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Dzień dobry!</span> <span class="yarn-meta">#line:0039ce8 </span>
    <span class="yarn-cmd">&lt;&lt;jump greengrocer_bonjour&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-greengrocer-bonjour"></a>

## greengrocer_bonjour

<div class="yarn-node" data-title="greengrocer_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cześć! Sprzedaję owoce i warzywa. Jestem warzywniakiem.</span> <span class="yarn-meta">#line:041ade1 </span>
<span class="yarn-line">Moje produkty są zawsze świeże!</span> <span class="yarn-meta">#line:0969b87 </span>
<span class="yarn-cmd">&lt;&lt;card  person_greengrocer zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump greengrocer_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-payment-done"></a>

## grocer_payment_done

<div class="yarn-node" data-title="grocer_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">tags: actor=WOMAN_OLD, do_not_translate, noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#line:0ce6f8a </span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:0e8ec1b </span>
<span class="yarn-line">Bonne journée!</span> <span class="yarn-meta">#line:062029a </span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Grocer&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $grocer_completed = true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-dontsell"></a>

## grocer_dontsell

<div class="yarn-node" data-title="grocer_dontsell">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">tags: actor=WOMAN_OLD</span>
<span class="yarn-header-dim">action: OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Przykro mi, ale tego nie sprzedaję.</span> <span class="yarn-meta">#line:0977493 </span>

</code>
</pre>
</div>

<a id="ys-node-grocer-question"></a>

## grocer_question

<div class="yarn-node" data-title="grocer_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">tags: actor=WOMAN_OLD</span>
<span class="yarn-header-dim">actor: OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Co chcesz kupić?</span> <span class="yarn-meta">#line:0c36100 </span>
<span class="yarn-line">Pomidory, pomarańcze i cytryny</span> <span class="yarn-meta">#line:0d6dabd </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Chleb</span> <span class="yarn-meta">#line:03eeda4 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Mleko</span> <span class="yarn-meta">#line:097fca2 </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Sól, pieprz i olej</span> <span class="yarn-meta">#line:0068f15 </span>
    <span class="yarn-cmd">&lt;&lt;jump grocer_pay_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-shop-grocer"></a>

## shop_grocer

<div class="yarn-node" data-title="shop_grocer">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">tags: actor=WOMAN_OLD, noRepeatLastLine</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $grocer_completed&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: You already bought from me!]</span> <span class="yarn-meta">#line:004d2b1 </span>
<span class="yarn-line">[MISSING TRANSLATION: Try another shop.]</span> <span class="yarn-meta">#line:0b4ebe1 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:0dd992c </span>
    <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Dzień dobry!</span> <span class="yarn-meta">#line:0623f71 </span>
    <span class="yarn-cmd">&lt;&lt;jump grocer_bonjour&gt;&gt;</span>
<span class="yarn-line">Książka!</span> <span class="yarn-meta">#line:0b4db3b </span>
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
<span class="yarn-header-dim">tags: actor=WOMAN_OLD</span>
<span class="yarn-header-dim">actor: OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cześć! Sprzedaję przyprawy i produkty spożywcze. Jestem sklepem spożywczym.</span> <span class="yarn-meta">#line:0ffbfa4 </span>
<span class="yarn-line">Moje produkty możesz wykorzystać w wielu przepisach.</span> <span class="yarn-meta">#line:0c6a554 </span>
<span class="yarn-cmd">&lt;&lt;card  person_grocer zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump grocer_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-pirate"></a>

## pirate

<div class="yarn-node" data-title="pirate">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pirates</span>
<span class="yarn-header-dim">tags: actor=MAN_BIG</span>
<span class="yarn-header-dim">actor: CRAZY_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $pirate_completed&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Now go! I have to prepare for my next voyage.]</span> <span class="yarn-meta">#line:04a0605 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Ahoj! Witamy na pokładzie.</span> <span class="yarn-meta">#line:04ee922 </span>
<span class="yarn-line">Pochodzimy z Saint-Malo i przepłynęliśmy całe morza, żeby tu dotrzeć.</span> <span class="yarn-meta">#line:056c70d </span>
<span class="yarn-line">Ludzie nazywają nas piratami, ale byliśmy korsarzami.</span> <span class="yarn-meta">#line:0f764a6 </span>
<span class="yarn-cmd">&lt;&lt;jump pirate_activity&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-pirate-activity"></a>

## pirate_activity

<div class="yarn-node" data-title="pirate_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: pirates</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Uzupełnij obraz.</span> <span class="yarn-meta">#line:08396f2 </span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_pirate activity_pirate_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-pirate-done"></a>

## activity_pirate_done

<div class="yarn-node" data-title="activity_pirate_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pirates</span>
<span class="yarn-header-dim">tags: actor=MAN_BIG</span>
<span class="yarn-header-dim">actor: CRAZY_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Pracowaliśmy dla króla Francji.</span> <span class="yarn-meta">#line:0af3bba </span>
<span class="yarn-line">W przeszłości kradliśmy przedmioty wrogom króla, ale te dni już minęły.</span> <span class="yarn-meta">#line:0b61715 </span>
<span class="yarn-line">[MISSING TRANSLATION: You should pay for what you need]</span> <span class="yarn-meta">#line:0209784 </span>
<span class="yarn-line">Powinieneś zapłacić za to, czego potrzebujesz, aby sklepy mogły pozostać otwarte!</span> <span class="yarn-meta">#line:02c49d0 </span>
<span class="yarn-cmd">&lt;&lt;set $pirate_completed = true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chef"></a>

## chef

<div class="yarn-node" data-title="chef">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if GetCollectedItem("COLLECT_THE_INGREDIENTS") == 9&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump chef_ingredients_done&gt;&gt;</span>
&lt;&lt;elseif GetCollectedItem("COLLECT_THE_INGREDIENTS") &gt; 0 &gt;&gt;
    <span class="yarn-cmd">&lt;&lt;jump chef_not_enough&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump chef_welcome&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chef-welcome"></a>

## chef_welcome

<div class="yarn-node" data-title="chef_welcome">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour! Witamy w Marsylii nad Morzem Śródziemnym!</span> <span class="yarn-meta">#line:02548dd </span>
<span class="yarn-line">Chcę przygotować dla Ciebie specjalne danie: zupę rybną bouillabaisse!</span> <span class="yarn-meta">#line:0c65de3 </span>
<span class="yarn-cmd">&lt;&lt;card bouillabaisse zoom&gt;&gt;</span>
<span class="yarn-line">Ale potrzebuję kilku składników.</span> <span class="yarn-meta">#line:0623733 </span>
<span class="yarn-cmd">&lt;&lt;jump task_ingredients&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-ingredients"></a>

## task_ingredients

<div class="yarn-node" data-title="task_ingredients">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Proszę, idź na rynek i kup je dla mnie.</span> <span class="yarn-meta">#line:06602c6 </span>
<span class="yarn-cmd">&lt;&lt;task_start COLLECT_THE_INGREDIENTS task_ingredients_done&gt;&gt;</span>
<span class="yarn-line">Pamiętaj o manierach!</span> <span class="yarn-meta">#line:02819db </span>
<span class="yarn-line">Powiedz „Bonjour”, aby powitać kogoś,</span> <span class="yarn-meta">#line:04c9d69 </span>
<span class="yarn-line">i "Merci" jako podziękowanie.</span> <span class="yarn-meta">#line:022fd8f </span>

</code>
</pre>
</div>

<a id="ys-node-task-ingredients-desc"></a>

## task_ingredients_desc

<div class="yarn-node" data-title="task_ingredients_desc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Zbierz wszystkie składniki potrzebne do przepisu.</span> <span class="yarn-meta">#line:00849bb </span>
<span class="yarn-line">Są to: chleb, ryba, pomarańcza, cytryna, pomidor, mleko, pieprz i sól, olej.</span> <span class="yarn-meta">#line:03fbee1 </span>

</code>
</pre>
</div>

<a id="ys-node-chef-not-enough"></a>

## chef_not_enough

<div class="yarn-node" data-title="chef_not_enough">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Potrzebuję więcej składników!</span> <span class="yarn-meta">#line:0c5c6ac </span>
<span class="yarn-line">Upewnij się, że porozmawiasz ze wszystkimi osobami na rynku.</span> <span class="yarn-meta">#line:0060c01 </span>

</code>
</pre>
</div>

<a id="ys-node-task-ingredients-done"></a>

## task_ingredients_done

<div class="yarn-node" data-title="task_ingredients_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Masz wszystkie składniki! Wróć do szefa kuchni.</span> <span class="yarn-meta">#line:05398f2 </span>

</code>
</pre>
</div>

<a id="ys-node-chef-ingredients-done"></a>

## chef_ingredients_done

<div class="yarn-node" data-title="chef_ingredients_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wspaniale! Masz wszystko.</span> <span class="yarn-meta">#line:0257fc7 </span>
<span class="yarn-line">Byłeś bardzo uprzejmy.</span> <span class="yarn-meta">#line:0112e25 </span>
<span class="yarn-line">Przygotujmy teraz naszą ucztę!</span> <span class="yarn-meta">#line:0051174 </span>
<span class="yarn-cmd">&lt;&lt;jump activity_match_ingredients&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match-ingredients"></a>

## activity_match_ingredients

<div class="yarn-node" data-title="activity_match_ingredients">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">tags: activity</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dopasuj każdy przedmiot do właściwego sprzedawcy.</span> <span class="yarn-meta">#line:0a6e106 </span>
<span class="yarn-cmd">&lt;&lt;activity match_ingredients activity_match_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match-done"></a>

## activity_match_done

<div class="yarn-node" data-title="activity_match_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: chef</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Gratulacje! Udało Ci się dopasować wszystkie elementy.</span> <span class="yarn-meta">#line:01648b2 </span>
<span class="yarn-line">Teraz czas na ugotowanie zupy rybnej!</span> <span class="yarn-meta">#line:0f0f617 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-fisher-pay-activity"></a>

## fisher_pay_activity

<div class="yarn-node" data-title="fisher_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: fisher</span>
<span class="yarn-header-dim">tags: actor=MAN_OLD</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wybierz kwotę wystarczającą do zapłaty.</span> <span class="yarn-meta">#line:0995020 </span>
<span class="yarn-cmd">&lt;&lt;activity money_fishmonger fisher_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-pay-activity"></a>

## cheesemonger_pay_activity

<div class="yarn-node" data-title="cheesemonger_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group:cheesemonger</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wybierz kwotę wystarczającą do zapłaty.</span> <span class="yarn-meta">#line:0f44ea7 </span>
<span class="yarn-cmd">&lt;&lt;activity money_cheesemonger cheesemonger_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-pay-activity"></a>

## grocer_pay_activity

<div class="yarn-node" data-title="grocer_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">tags: actor=WOMAN_OLD</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wybierz kwotę wystarczającą do zapłaty.</span> <span class="yarn-meta">#line:0c80f9e </span>
<span class="yarn-cmd">&lt;&lt;activity money_grocer grocer_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-greengrocer-pay-activity"></a>

## greengrocer_pay_activity

<div class="yarn-node" data-title="greengrocer_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: greengrocer</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wybierz kwotę wystarczającą do zapłaty.</span> <span class="yarn-meta">#line:08fc94e </span>
<span class="yarn-cmd">&lt;&lt;activity money_greengrocer greengrocer_payment_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-notunderstand"></a>

## baker_notunderstand

<div class="yarn-node" data-title="baker_notunderstand">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: baker</span>
<span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Przepraszam, chyba nie rozumiem...</span> <span class="yarn-meta">#line:0db5121 </span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-notunderstand"></a>

## cheesemonger_notunderstand

<div class="yarn-node" data-title="cheesemonger_notunderstand">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Przepraszam, chyba nie rozumiem...</span> <span class="yarn-meta">#line:01a0ec9 </span>

</code>
</pre>
</div>

<a id="ys-node-grocer-notunderstand"></a>

## grocer_notunderstand

<div class="yarn-node" data-title="grocer_notunderstand">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">tags: actor=WOMAN_OLD</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Przepraszam, chyba nie rozumiem...</span> <span class="yarn-meta">#line:0a30381 </span>

</code>
</pre>
</div>

<a id="ys-node-talk-dont-understand"></a>

## talk_dont_understand

<div class="yarn-node" data-title="talk_dont_understand">
<pre class="yarn-code" style="--node-color:orange"><code>
<span class="yarn-header-dim">tags: detour</span>
<span class="yarn-header-dim">color: orange</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Przepraszam, chyba nie rozumiem...</span> <span class="yarn-meta">#line:0f9044b </span>
<span class="yarn-line">Co??</span> <span class="yarn-meta">#line:09682b7 </span>
<span class="yarn-line">Co?</span> <span class="yarn-meta">#line:0c1b3e0 </span>

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
<span class="yarn-line">Przykro mi, ale tego nie sprzedaję.</span> <span class="yarn-meta">#line:08700b0 </span>

</code>
</pre>
</div>

<a id="ys-node-item-bread"></a>

## item_bread

<div class="yarn-node" data-title="item_bread">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_bread&gt;&gt;</span>
<span class="yarn-line">Chleb</span> <span class="yarn-meta">#line:08e101e </span>

</code>
</pre>
</div>

<a id="ys-node-item-fish"></a>

## item_fish

<div class="yarn-node" data-title="item_fish">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_fish&gt;&gt;</span>
<span class="yarn-line">Ryba</span> <span class="yarn-meta">#line:0feed79 </span>

</code>
</pre>
</div>

<a id="ys-node-item-crab"></a>

## item_crab

<div class="yarn-node" data-title="item_crab">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_crab&gt;&gt;</span>
<span class="yarn-line">Krab</span> <span class="yarn-meta">#line:0c81979 </span>

</code>
</pre>
</div>

<a id="ys-node-item-orange"></a>

## item_orange

<div class="yarn-node" data-title="item_orange">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_orange&gt;&gt;</span>
<span class="yarn-line">Pomarańczowy</span> <span class="yarn-meta">#line:0c0fa04 </span>

</code>
</pre>
</div>

<a id="ys-node-item-lemon"></a>

## item_lemon

<div class="yarn-node" data-title="item_lemon">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_lemon&gt;&gt;</span>
<span class="yarn-line">Cytrynowy</span> <span class="yarn-meta">#line:0c6b991 </span>

</code>
</pre>
</div>

<a id="ys-node-item-tomato"></a>

## item_tomato

<div class="yarn-node" data-title="item_tomato">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_tomato&gt;&gt;</span>
<span class="yarn-line">Pomidor</span> <span class="yarn-meta">#line:0a6782d </span>

</code>
</pre>
</div>

<a id="ys-node-item-milk"></a>

## item_milk

<div class="yarn-node" data-title="item_milk">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_milk&gt;&gt;</span>
<span class="yarn-line">Mleko</span> <span class="yarn-meta">#line:0acd781 </span>

</code>
</pre>
</div>

<a id="ys-node-item-pepper-salt"></a>

## item_pepper_salt

<div class="yarn-node" data-title="item_pepper_salt">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_pepper_salt&gt;&gt;</span>
<span class="yarn-line">Sól i pieprz</span> <span class="yarn-meta">#line:07bbcb0 </span>

</code>
</pre>
</div>

<a id="ys-node-item-oil"></a>

## item_oil

<div class="yarn-node" data-title="item_oil">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR, item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_olive_oil&gt;&gt;</span>
<span class="yarn-line">Oliwa z oliwek</span> <span class="yarn-meta">#line:0156410 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-tourist"></a>

## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: tourists </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cześć! Przyjeżdżam z Paryża.</span> <span class="yarn-meta">#line:00a142a </span>
<span class="yarn-line">Jedzenie tutaj jest niesamowite!</span> <span class="yarn-meta">#line:0374dff </span>
<span class="yarn-line">Uwielbiam morze!</span> <span class="yarn-meta">#line:0980627 </span>
<span class="yarn-line">Na rynku jest tak wesoło!</span> <span class="yarn-meta">#line:0d92388 </span>
<span class="yarn-line">Bouillabaisse to moje ulubione danie!</span> <span class="yarn-meta">#line:07c080f </span>

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
<span class="yarn-line">Muszę kupić świeże składniki.</span> <span class="yarn-meta">#line:0baa74d </span>
<span class="yarn-line">Na rynku można kupić najlepsze produkty.</span> <span class="yarn-meta">#line:042c6f0 </span>
<span class="yarn-line">Świeża ryba jest najlepsza!</span> <span class="yarn-meta">#line:01269c6 </span>
<span class="yarn-line">Nie mogę się doczekać, aż przygotuję pyszny posiłek!</span> <span class="yarn-meta">#line:0fc5cd3 </span>

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
<span class="yarn-line">Jedno euro to 100 centów.</span> <span class="yarn-meta">#line:0e6c526 </span>
<span class="yarn-cmd">&lt;&lt;card currency_euro&gt;&gt;</span>
<span class="yarn-line">Uprzejme słowa i odpowiednie monety: idealnie!</span> <span class="yarn-meta">#line:07a5934 </span>
<span class="yarn-cmd">&lt;&lt;card currency_euro&gt;&gt;</span>
<span class="yarn-line">Każda waluta euro ma inną wielkość.</span> <span class="yarn-meta">#line:021819a </span>
<span class="yarn-cmd">&lt;&lt;card currency_euro&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-recipe"></a>

## spawned_recipe

<div class="yarn-node" data-title="spawned_recipe">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: buyers </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bouillabaisse to zupa rybna łącząca wiele gatunków ryb.</span> <span class="yarn-meta">#line:0b6981c </span>
<span class="yarn-cmd">&lt;&lt;card bouillabaisse&gt;&gt;</span>
<span class="yarn-line">Świeży pomidor dodaje smaku.</span> <span class="yarn-meta">#line:0477851 </span>
<span class="yarn-cmd">&lt;&lt;card food_tomato&gt;&gt;</span>
<span class="yarn-line">Oliwa z oliwek dodaje smaku.</span> <span class="yarn-meta">#line:060644d </span>
<span class="yarn-cmd">&lt;&lt;card food_olive_oil&gt;&gt;</span>
<span class="yarn-line">Chleb świetnie nadaje się do maczania zup.</span> <span class="yarn-meta">#line:04e69ac </span>
<span class="yarn-cmd">&lt;&lt;card food_bread&gt;&gt;</span>

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
<span class="yarn-line">Piekarz piecze chleb.</span> <span class="yarn-meta">#line:0606dc5 </span>
<span class="yarn-cmd">&lt;&lt;card person_baker&gt;&gt;</span>
<span class="yarn-line">Sprzedawca ryb sprzedaje ryby i kraby.</span> <span class="yarn-meta">#line:0ca5a9b </span>
<span class="yarn-cmd">&lt;&lt;card person_fishmonger&gt;&gt;</span>
<span class="yarn-line">Sprzedawca sera sprzedaje ser i mleko.</span> <span class="yarn-meta">#line:0b5b1c5 </span>
<span class="yarn-cmd">&lt;&lt;card person_cheesemonger&gt;&gt;</span>
<span class="yarn-line">Sprzedawca warzyw i owoców sprzedaje warzywa.</span> <span class="yarn-meta">#line:03b9a2e </span>
<span class="yarn-cmd">&lt;&lt;card person_greengrocer&gt;&gt;</span>

</code>
</pre>
</div>


