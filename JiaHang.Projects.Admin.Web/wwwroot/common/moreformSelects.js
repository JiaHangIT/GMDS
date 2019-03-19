if (typeof (Array.prototype.indexOf) === 'undefined') {
    Array.prototype.indexOf = function (val) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] === val) return i;
        }
        return -1;
    };
}
if (typeof (Array.prototype.remove) === 'undefined') {
    Array.prototype.remove = function (val) {
        var index = this.indexOf(val);
        if (index > -1) {
            this.splice(index, 1);
        }
    };
}

/**
 * layui formSelects 多选联动(去重复)
 * 
 * moreformSelects({
 *     "data-source": cityData, //[]
 *     ids: ['brand', 'region', 'city', 'dm', 'store'],
 *     maps: [
 *         //一般value为长路径
 *         { "name": "n", "value": "v", "children": "cc" },
 *         { "name": "n", "value": "v", "children": "cc" },
 *         { "name": "n", "value": "v", "children": "cc" },
 *         { "name": "n", "value": "v", "children": "cc" },
 *         { "name": "n", "value": "v", "children": undefined },
 *     ]
 * });
 * 
 * @param {any} options
 */
function moreformSelects(options) {
    var $ = layui.$;
    var formSelects = layui.formSelects;

    var uiLen = options.ids.length || 0;
    var cobj = new Array(uiLen);

    if (uiLen < 1) return;

    // func: (i, item)=> {};
    function aFind(a, func) {
        for (var i = 0; i < a.length; i++) {
            if (func(a[i], i)) return { i: i, val: a[i] };
        }
        return { i: -1 };
    }

    function removeAt(a, i) {
        if (i > -1) a.splice(i, 1);
    }

    function addToSel(sel, name, vals) {
        sel[name] = sel[name] || [];
        if ($.isArray(vals)) sel[name].push.apply(sel[name], vals);
        else sel[name].push.apply(sel[name], vals.split(','));
    }

    function delSel(sel, name, vals) {
        if (!sel[name]) return;
        //var len = sel[name].length;
        if ($.isArray(vals)) $.each(vals, function (_, v) { sel[name].remove(v); });
        else $.each(vals.split(','), function (_, v) { sel[name].remove(v); });
        //console.log('del sel["' + name + '"] ' + (len > sel[name].length ? 'true' : 'false'));
        !sel[name].length && (delete sel[name]);
    }

    function getSel(sel) {
        var a = [];
        for (var name in sel)
            a.push(sel[name]);
        return a;
    }

    for (var _i = 0; _i < uiLen; _i++) {
        if (_i == 0) {
            cobj[_i] = {
                i: _i,
                init: function () {
                    var i = this.i, is_end = (i == uiLen - 1);
                    formSelects.data(options.ids[i], 'local', {
                        arr: $.map(options['data-source'], function (x) {
                            return { "name": x[options['maps'][i].name], "value": x[options['maps'][i].value], "src_item": [x] };
                        })
                    });
                    /** formSelects.on ：
                     * isAdd==true时，val为完整结构
                     * isAdd==false时，val为非完整结构 {name:'',value:''}
                     */
                    formSelects.on(options.ids[i], function (id, vals, val, isAdd, isDisabled) {
                        if (is_end) return;
                        if (!isAdd) cobj[i + 1].rmItems($.grep(vals, function (x) { return x.src_item[0][options['maps'][i].value] == val.value }));
                        else cobj[i + 1].addItems(val.src_item);
                    });
                }
            };
        } else {
            cobj[_i] = {
                i: _i,
                a: [],
                sel: {},
                init: function () {
                    var self = this, a = self.a, i = self.i, is_end = (i == uiLen - 1);

                    // fill data
                    formSelects.data(options.ids[i], 'local', { arr: a });
                    // reset selected
                    formSelects.value(options.ids[i], getSel(self.sel), true);
                    // bind selected event
                    formSelects.on(options.ids[i], function (id, vals, val, isAdd, isDisabled) {
                        if (!isAdd) {
                            var item = $.grep(a, function (x) {
                                if (x.value == val.value) {
                                    delSel(self.sel, x.name, x.vals || x.value);
                                    return true;
                                }
                                return false;
                            });
                            if (!is_end) cobj[i + 1].rmItems(item);
                        } else {
                            addToSel(self.sel, val.name, val.vals);
                            if (!is_end) cobj[i + 1].addItems(val.src_item);
                        }
                    });
                },
                addItems: function (pvals) {
                    var self = this, a = self.a, i = self.i, is_end = (i == uiLen - 1);
                    /** a.push({ "name": pval[options['maps'][i - 1].name], "type": "optgroup", "p_group": pval[options['maps'][i - 1].value] }); //*/
                    $.each(pvals, function (_, pval) {
                        $.each(pval[options['maps'][i - 1].children], function (_, x) {
                            var _x = aFind(a, function (_a) { return _a.name === x[options['maps'][i].name]; });
                            if (_x.i == -1) { // 非重复add
                                a.push({
                                    "name": x[options['maps'][i].name],
                                    "value": x[options['maps'][i].value],
                                    "vals": [x[options['maps'][i].value]],
                                    "src_item": [x],
                                    "p_group": [pval[options['maps'][i - 1].value]],
                                });
                            } else { // 重复add
                                _x.val.vals.push(x[options['maps'][i].value]);
                                _x.val.value = _x.val.vals.join(',');
                                _x.val.src_item.push(x);
                                _x.val.p_group.push(pval[options['maps'][i - 1].value]);

                                if (self.sel[x[options['maps'][i].name]]) { // 重复add selected
                                    addToSel(self.sel, x[options['maps'][i].name], x[options['maps'][i].value]);
                                    if (!is_end) cobj[i + 1].addItems([x]);
                                }
                            }
                        });
                    });
                    self.init();
                },
                rmItems: function (pvals) {
                    if (!pvals || !pvals.length) return;
                    var self = this, i = self.i, is_end = (i == uiLen - 1), nxt = [];

                    // map parent node vals to array
                    var ps = $.map(pvals, function (x) { return x.vals || x.value; });

                    var g = $.grep(self.a, function (x) {
                        $.each(ps, function (_, p) {
                            var j = x['p_group'].indexOf(p);
                            if (j > -1) {
                                var _src = aFind(x.src_item, function (_x) { return _x[options['maps'][i].value] == (p + '/' + x.name); });

                                removeAt(x['p_group'], j);  //del p_group
                                x.vals.remove(p + '/' + x.name); //del vals ...
                                x.value = x.vals.join(',');
                                removeAt(x.src_item, _src.i); //del src_item

                                nxt.push({
                                    name: x.name,
                                    //vals: [p + '/' + x.name],
                                    'value': (p + '/' + x.name),
                                    //'p_group': [p],
                                    //'src_item': [_src.val],
                                });
                            }
                            delSel(self.sel, x.name, p + '/' + x.name); //del sel 
                        });
                        return x['p_group'].length <= 0;
                    });
                    $.each(g, function (_, x) {
                        self.a.remove(x); //del empty p_group nodes
                    });

                    self.init();

                    if (!is_end) cobj[i + 1].rmItems(nxt); //remove nexts 
                }
            };
        }
    }

    for (var i = 0; i < uiLen; i++)
        cobj[i].init();

    return {
        _cobj: cobj, //for test
        getAllDatas: function (format) {
            var a = [];
            $.each(options.ids, function (_, id) {
                a.push(formSelects.value(id, format));
            });
            return a;
        }
    };
}