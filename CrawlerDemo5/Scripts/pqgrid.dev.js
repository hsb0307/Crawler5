/**
 * ParamQuery Grid a.k.a. pqGrid v1.0.3
 * 
 * Copyright (c) 2012 Paramvir Dhindsa (http://paramquery.com)
 * Released under MIT license
 * http://paramquery.com/license
 * 
 */     	
(function($){
	$.adapter={
		xmlToArray:function(data,obj){
			var itemParent=obj.itemParent;
			var itemNames=obj.itemNames;
			var arr=[];
			var $items=$(data).find(itemParent);
			$items.each(function(i,item){
				var $item=$(item);
				var arr2=[];
				$(itemNames).each(function(j,itemName){
					arr2.push($item.find(itemName).text());	
				})
				arr.push(arr2);
			})
			return arr;
		},
		tableToArray:function(tbl){
			var $tbl=$(tbl);
			var colModel=[];
			var data=[];
			var cols=[];
			var widths=[];
			var $trfirst=$tbl.find("tr:first");
			var $trsecond=$tbl.find("tr:eq(1)");
			$trfirst.find("th,td").each(function(i,td){
				var $td=$(td);
				var title=$td.html();
				var width=$td.width();
				var dataType="string";
				var $tdsec=$trsecond.find("td:eq("+i+")");
				var val=$tdsec.text();
				var align=$tdsec.attr("align");
				val=val.replace(/,/g,"");
				if(parseInt(val)==val && (parseInt(val)+"").length == val.length){
					dataType="integer";
				}
				else if(parseFloat(val)==val ){
					dataType="float";
				}
				var obj={title:title,width:width,dataType:dataType,align:align};
				colModel.push(obj);
			})
			$tbl.find("tr").each(function(i,tr){
				if(i==0)return;
				var $tr=$(tr);
				var arr2=[];
				$tr.find("td").each(function(j,td){
					arr2.push($.trim($(td).html()));
				})
				data.push(arr2);
			})
			return {data:data,colModel:colModel};
		}
	}
})(jQuery);
/**
 * ParamQuery Pager a.k.a. pqPager
 */     	
(function($){
var fnPG={};
fnPG.options={
	currentPage:0,
	totalPages:0,
	totalRecords:0,
	msg:"",
	rPPOptions:[10,20,30,40,50,100],
	rPP:20,
	change:null
};
fnPG._create=function(){
	var that=this;
	this.element.addClass("pq-pager").css({});
	this.first = $( "<button type='button' title='First Page'></button>", {
	})
	.appendTo( this.element )
	.button({
		icons: {
			primary:"pq-page-first"
		},text:false
	}).bind("click.paramquery",function(evt){
		if(that.options.currentPage>1){
			if ( that._trigger( "change", evt, {
				curPage: 1
			} ) !== false ) {
				that.option( {currentPage:1} );
			}
		}					
	});
	this.prev=$( "<button type='button' title='Previous Page'></button>")
	.appendTo( this.element )
	.button({icons:{primary:"pq-page-prev"},text:false}).bind("click",function(evt){
		if(that.options.currentPage>1){
			var currentPage=that.options.currentPage-1;
			if ( that._trigger( "change", evt,{
				curPage: currentPage
			} ) !== false ) {
				that.option( {currentPage:currentPage} );
			}						
		}
	});
	$("<span class='pq-separator'></span>").appendTo(this.element);
	$( "<span>Page </span>", {					
	})
	.appendTo( this.element )
	this.page=$( "<input type='text' tabindex='0' />")
	.appendTo( this.element )
	.bind("change",function(evt){
		var $this=$(this)
		var val=$this.val();
		if(isNaN(val)||val<1){
			$this.val(that.options.currentPage)
			return false;
		}
		val=parseInt(val);
		if(val>that.options.totalPages){
			$this.val(that.options.currentPage);
			return false;						
		}
		if ( that._trigger( "change", evt, {
			curPage: val
		}) !== false ) {
			that.option( {currentPage:val} );
		}				
		else{
			$this.val(that.options.currentPage);
			return false;												
		}		
	})
	$( "<span>of </span>", {					
	})
	.appendTo( this.element )
	this.$total=$( "<span class='total'></span>", {					
	})
	.appendTo( this.element )
	$("<span class='pq-separator'></span>").appendTo(this.element);
	this.next=$( "<button type='button' title='Next Page'></button>")
	.appendTo( this.element )
	.button({icons:{primary:"pq-page-next"},text:false}).bind("click",function(evt){
		var val=that.options.currentPage+1;
		if ( that._trigger( "change", evt, {curPage: val} ) !== false ) {			
			that.option( {currentPage:val} );
		}				
	});
	this.last=$( "<button type='button' title='Last Page'></button>")
	.appendTo( this.element )
	.button({icons:{primary:"pq-page-last"},text:false}).bind("click",function(evt){
		var val=that.options.totalPages;
		if ( that._trigger( "change", evt, {curPage: val} ) !== false ) {			
			that.option( {currentPage:val} );
		}									
	});
	$("<span class='pq-separator'></span>").appendTo(this.element);
	$("<span>Records per page: </span>")
	.appendTo(this.element)
	this.$rPP=$("<select></select>")
	.appendTo(this.element)
	.change(function(evt){
		var val=$(this).val();
		if (that._trigger("change", evt,{rPP: val}) !== false) {
			that.options.rPP=val;
		}
	})
	$("<span class='pq-separator'></span>").appendTo(this.element);
	this.$refresh=$("<button type='button' title='Refresh'></button>")
	.appendTo(this.element)
	.button({icons:{primary:"pq-refresh"},text:false}).bind("click",function(evt){		
		if ( that._trigger( "refresh", evt ) !== false ) {			
		}				
	});
	$("<span class='pq-separator'></span>").appendTo(this.element);
	this.$msg=$("<span class='pq-pager-msg'></span>")
	.appendTo( this.element )
	this._refresh();
}
fnPG._refresh=function(){
	var sel=(this.$rPP);
	sel.empty();
	var opts = this.options.rPPOptions;
	for(var i=0;i<opts.length;i++){
        var opt=document.createElement("option");
        opt.text=opts[i];
        opt.value=opts[i];
        opt.setAttribute("value",opts[i]);
        opt.innerHTML=opts[i];
		sel.append(opt)
	}				
	sel.find("option[value="+this.options.rPP+"]").attr("selected",true)
	if(this.options.currentPage>=this.options.totalPages){
		this.next.button({disabled:true});
		this.last.button({disabled:true});
	}
	else{
		this.next.button({disabled:false});
		this.last.button({disabled:false});					
	}
	if(this.options.currentPage<=1){
		this.first.button({disabled:true});
		this.prev.button({disabled:true});
	}		
	else{
		this.first.button({disabled:false});
		this.prev.button({disabled:false});					
	}
	this.page.val(this.options.currentPage)
	this.$total.text(this.options.totalPages);		
	if(this.options.totalRecords>0){
		var rPP = this.options.rPP;
		var currentPage = this.options.currentPage;
		var totalRecords = this.options.totalRecords;
		var begIndx = (currentPage-1)*rPP;
		var endIndx = currentPage*rPP;
		if(endIndx>totalRecords){
			endIndx = totalRecords;
		}
		this.$msg.html("Displaying "+(begIndx+1)+" to "
			+(endIndx)+" of "+totalRecords+" items.")		
	}
	else{
		this.$msg.html("");
	}
}
fnPG._destroy=function(){
	this.element.empty().removeClass("pq-pager").enableSelection();
	_super();
}
fnPG._setOption=function(key,value){
	if(key=="currentPage"||key=="totalPages")value=parseInt(value);	
	$.Widget.prototype._setOption.call( this, key, value );				
}
fnPG._setOptions=function(){
	$.Widget.prototype._setOptions.apply( this, arguments );
	this._refresh();				
}
	$.widget("paramquery.pqPager",fnPG);	
})(jQuery);
/**
	direction:'vertical'
		ele.html("<div class='left-btn pq-sb-btn'></div>\
				var new_top = clickY-top_this;						
				that.$slider.css("top",clickY-top_this-that.$slider.height());
			return this;
		}				
		this._setSliderBgLength();
		this.scroll_space =this.length-34-this.slider_length;
		this._setSliderBgLength();
	}
	}
			that._updateCurPosAndTrigger(evt,top);
	}
        this.element.empty(); 
        this.element.css('height', "");
        for (var i = 1; i < $tds.length; i++) {
        this.colModel = this.options.colModel ? this.options.colModel : [];
        this.$footer = $("div.pq-grid-footer", this.element); 
        this.$cont_o = $("div.pq-cont-right", this.$grid_right);
            var obj = that._findCellFromEvtCoords(evt);
                return true;
                that._editCell($td);
            if (evt.wheelDelta) {
                num = evt.detail * -1 / 3;
                if(that.init!=obj.cur_pos){
        }
        this._refresh();
            this._refreshDataFromDataModel();
				var i1=i;
        }
            if (DM.curPage > DM.totalPages) {
            }
            this.xhr.abort();
                    DM.data = retObj.data;
                        that._refreshSortingDataAndView(true);
		var newarr=[];
        }, '_generateTables');
        } else if (this.$td_edit) {
        } else if (this.$td_focus) { 
			this._boundCell(rowIndx, colIndx); 
        }
        this._refreshHeaderSortIcons();
        this._setHScrollWidth(); 
        this._refreshPager();
                this._setHScrollWidth();
		}
        this._refresh(); 
    }    
            this.$div_focus = $("<div class='pq-cell-selected-border'></div>")
            this.$div_focus.css({
        if ($tr == null || $tr.length == 0) {
        var ht = $tr[0].offsetHeight - 4;
        return true;
                var $trs = this.$tbl.children().children("tr");
        }
            var cur_pos = colIndx - this.freezeCols - this._calcNumHiddenUnFrozens(colIndx);
                var $tds = $td.parent("tr").children("td");
            this._boundCell(null, null, this.$td_edit);
        if ($td == null || $td.length == 0) {
        return true;
            }
            return false;
        var rowIndx = parseInt($tr.attr("pq-row-indx"));
                that.saveEditCell();
        }
        this.$hscroll.pqScrollBar("option", "length", wd);
        }
        }
        if (this.$freezeLine) this.$freezeLine.remove();
        $pQuery_drag.draggable("option", 'containment', [cont_left, 0, cont_right, 0]);
				continue;				
            }
            });
                that._getDragHelper(evt, ui);
            }
                    continue;
                    strStyle = "width:" + this.widths[j1] + "px;";						
                if (this.colModel[j1].align == "right") {
                if (this.colModel[j1].align == "right") {
            this.$tbl = $(str);
        var cls = "pq-td-div";
        if (data == null || data.length == 0) {
                var val2 = obj2[indx].replace(/,/g, "");
    }