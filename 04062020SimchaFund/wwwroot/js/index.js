
$(() => {

    //    $("#add-simcha").on('click', () => {
    //        $('.modal').modal();
    //    })




    ////    $("#add-contr").on('click', () => {
    ////        $("#contrib-modal").modal();
    ////    })    


    $("#add-simcha").on('click', function () {
        $("#simcha-modal").modal();
    });

    $("#add-contr").on('click', function () {
        $("#mod-title").text('New');
        $("#contr-modal").modal();
        $("#init-deposit").show();
        $("#dat-date").show();
        $("#txt-first").val('');
        $("#txt-last").val('');
        $("#txt-phone").val('');
        $("#txt-id").val('');


    });


    $('.form-contr-edit').on('click', function () {
        $("#mod-title").text('Edit');
        $("#form-contr").attr('action', '/Home/EditContributor');

        //preset the modals to the selected edit record values
        $("#init-deposit").hide();
        $("#dat-date").hide();

        $("#txt-date").val($(this).data('date'))//set it anyway even if it's not visible so that the ensurevalidity() will see it is set and enabel the submit button
        $("#txt-first").val($(this).data('first'));
        $("#txt-last").val($(this).data('last'));
        $("#txt-phone").val($(this).data('phone'));
        $("#txt-id").val($(this).data('id')); //set the hidden     


        let istrue = $(this).data('alwayscontr'); //this will grab the value of the data attribue as a string. 

        //this checks the checkbox based on what the data value is i.e. check it if data is true  uncheck if it's false
        $("#chk-include").prop('checked', istrue === "True");  //istrue==="True" translates to a boolean vs. the istrue which is a string    


        $("#contr-modal").modal();
    });
    /////////////// Enable the modal submit buttons only if required data is entered.

    function isValidContr() {
        const first = $("#txt-first").val();
        const last = $("#txt-last").val();
        const phone = $("#txt-phone").val();
        const date = $("#txt-date").val();
        console.log(date);
        return first && last && phone && date
    }

    function isValidDepo() {
        const amt = $("#txt-amt").val();
        const depodate = $("#date").val();
      
        return amt && depodate;
    }
    function EnableSubmit() {
        $("#btn-submit-contr").prop('disabled', !isValidContr());
        $("#btn-submit-depo").prop('disabled', !isValidDepo());
    }

    $("input").on('click', function () {
        EnableSubmit();
        
    })

    ////////////////////////////////////////////////////

    $('.form-depo-add').on('click', function () { 
        console.log($(this).data('id'))
        
        $("#txt-contrid").val($(this).data('id'));
        $("#depo-title").text(`Deposit for ${$(this).data('name')}`);
        $("#depo-modal").modal();
    });



    ///Search
    $("#txt-search").on('keyup', function () {
        const search = $(this).val();
        $("table tr:gt(0)").each(function () {
            const tr = $(this);
            const name = tr.find('td:eq(1)').text();
            if (name.toLowerCase().indexOf(search.toLowerCase()) !== -1) { //if search string is found in the name fields
                tr.show();
            }
            else {
                tr.hide();
            }
                
        })
    })

    $("#btn-clear").on('click', function () {
        $("#txt-search").val("");
        $('tr').show();
    })
    
   
});