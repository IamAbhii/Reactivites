﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API.Controllers {
  [ApiController]
  [Route ("[controller]")]
  public class ValueController : ControllerBase {
    private readonly DataContext _context;
    public ValueController (DataContext context) {
      _context = context;
    }
 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Value>>> Get(){
        var values =await _context.Values.ToListAsync();
        return Ok(values);
    }

    [HttpGet ("{id}")]

    public async Task<ActionResult<Value>> Get (int id) {
        var value=await _context.Values.SingleOrDefaultAsync(x=>x.Id == id);
        return Ok(value);
    }

  }
}