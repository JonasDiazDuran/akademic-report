﻿using AkademicReport.Data;
using AkademicReport.Dto.UsuarioDto;
using AkademicReport.Models;
using AkademicReport.Utilities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using System.Collections.Generic;

namespace AkademicReport.Service.UsuarioServices
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        public UsuarioService(IMapper mapper, DataContext dataContext)
        {
            _mapper = mapper;
            _dataContext = dataContext;
        }

        public async Task<ServicesResponseMessage<string>> Create(UsuarioAddDto usuario)
        {
            try
            {
                var usuarios = await CargarUsuarios();
                var usuariodb = usuarios.Where(c => c.Correo == usuario.Correo).FirstOrDefault();
                if (usuariodb != null)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjUsuarioExiste };


                _dataContext.Usuarios.Add(_mapper.Map<Usuario>(usuario));
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjUsuarioInsertado };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjUsuarioExiste +  ex.ToString()};
            }
            
        }

        public async Task<ServicesResponseMessage<string>> Delete(int id)
        {
            try
            {
                var usuariodb = await _dataContext.Usuarios.FirstOrDefaultAsync(c => c.Id == id);
                if (usuariodb == null)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjNoRegistros };
                usuariodb.SoftDelete = 1;
                _dataContext.Entry(usuariodb).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjDelete };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString };
            }
        }

        public async Task<ServiceResponseData<List<UsuarioGetDto>>> GetAll()
        {
            try
            {
                var usuariosdb = await CargarUsuarios();
                if (usuariosdb.Count<1)
                    return new ServiceResponseData<List<UsuarioGetDto>>() {Status = 204};
                return new ServiceResponseData<List<UsuarioGetDto>>() { Status = 200, Data=  _mapper.Map<List<UsuarioGetDto>>(usuariosdb) };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<UsuarioGetDto>>() { Status = 500};
            }
        }

        public async Task<ServiceResponseData<UsuarioGetDto>> GetById(int id)
        {
            try
            {
                var usuarios = await CargarUsuarios();
                var usuariodb = _mapper.Map<UsuarioGetDto>(usuarios.Where(c => c.Id == id).FirstOrDefault());
                if (usuariodb==null)
                    return new ServiceResponseData<UsuarioGetDto>() { Status = 204 };
                return new ServiceResponseData<UsuarioGetDto>() { Status = 200, Data =  usuariodb };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<UsuarioGetDto>() { Status = 500 };
            }
        }

        public async Task<ServiceResponseData<List<UsuarioGetDto>>> GetByIdRecinto(int id)
        {
            try
            {
                var usuarios = await CargarUsuarios();
                var usuariodb = usuarios.Where(c => c.IdRecinto == id);
                if (usuariodb == null)
                    return new ServiceResponseData<List<UsuarioGetDto>>() { Status = 204 };
                return new ServiceResponseData<List<UsuarioGetDto>>() { Status = 200, Data = _mapper.Map<List<UsuarioGetDto>>(usuariodb) };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<UsuarioGetDto>>() { Status = 500 };
            }
        }

        public async Task<ServisResponseLogin<List<UsuarioGetDto>, string >> Login(UsuarioCredentialsDto credentials)
        {
            try
            {

                var usuariodb = await _dataContext.Usuarios.Where(c => c.Correo == credentials.correo && c.Contra == credentials.contra).Include(c => c.IdRecintoNavigation).Include(c => c.NivelNavigation).ToListAsync();
                if (usuariodb == null)
                    return new ServisResponseLogin<List<UsuarioGetDto>, string>() { Status = 204, Message = (_mapper.Map<List<UsuarioGetDto>>(usuariodb) , Msj.MsjCredencialesIncorrectas)};
                return new ServisResponseLogin<List<UsuarioGetDto>, string>() { Status = 200, Message = (_mapper.Map<List<UsuarioGetDto>>(usuariodb), "") };
            }
            catch (Exception ex)
            {
                return new ServisResponseLogin<List<UsuarioGetDto>, string>() { Status = 500, Message = (new List<UsuarioGetDto>(), Msj.MsjError + ex.ToString())};
            }
        }

        public async Task<ServicesResponseMessage<string>> Update(UsuarioUpdateDto usuario)
        {
            try
            {
                var usuariodb = await _dataContext.Usuarios.AsNoTracking().Where(c => c.Correo == usuario.Correo && c.Id==int.Parse(usuario.Id)).ToListAsync();
                if (usuariodb.Count>1)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjUsuarioExiste };
                var usuariodbForUpdate = await _dataContext.Usuarios.AsNoTracking().Where(c=>c.Id == int.Parse(usuario.Id)).FirstOrDefaultAsync();
                int idRecinto = usuariodbForUpdate.IdRecinto;
                usuariodbForUpdate = _mapper.Map<Usuario>(usuario);
                usuariodbForUpdate.IdRecinto = idRecinto;

                // _dataContext.Usuarios.Add(_mapper.Map<Usuario>(usuariodbForUpdate));
                _dataContext.Entry(usuariodbForUpdate).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync();

                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjUsuarioInsertado };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjUsuarioExiste + ex.ToString() };
            }
        }

        public async Task<List<Usuario>>CargarUsuarios()
        {
           
            return await _dataContext.Usuarios.Where(c=>c.SoftDelete==0).Include(c => c.IdRecintoNavigation).Include(c => c.NivelNavigation).ToListAsync();
        }
    }
}
