using System;
using System.Reflection;
using System.Collections.Generic;

namespace org.ncore.Common
{
    // TODO: Add xml comments for class
    public sealed class MemberMapper
    {
        public enum MappingTemplateEnum
        {
            Target,
            Source
        }

        public enum MemberTypeEnum
        {
            Field,
            Property
        }

        public static object Map( object source, object target )
        {
            return Map( source, target, MemberTypeEnum.Field, true, MappingTemplateEnum.Target );
        }

        public static object Map( object source, object target, MemberTypeEnum memberType )
        {
            return Map( source, target, memberType, true, MappingTemplateEnum.Target );
        }

        public static object Map( object source, object target, MemberTypeEnum memberType, bool allowUnmatchedMembers )
        {
            return Map( source, target, memberType, allowUnmatchedMembers, MappingTemplateEnum.Target );
        }

        public static object Map( object source, object target, MemberTypeEnum memberType, bool allowUnmatchedMembers, List<string> excludeMemberNames )
        {
            return Map( source, target, memberType, allowUnmatchedMembers, MappingTemplateEnum.Target, excludeMemberNames );
        }

        public static object Map( object source, object target, MemberTypeEnum memberType, bool allowUnmatchedMembers, MappingTemplateEnum mappingTemplate )
        {
            return Map( source, target, memberType, allowUnmatchedMembers, mappingTemplate, null );
        }

        public static object Map( object source, object target, MemberTypeEnum memberType, bool allowUnmatchedMembers, MappingTemplateEnum mappingTemplate, List<string> excludeMemberNames )
        {
            return _map( source, target, memberType, allowUnmatchedMembers, mappingTemplate, excludeMemberNames );
        }


        // TODO: This method is too long.  Refactor and decompose.  JF
        private static object _map( object source, object target, MemberTypeEnum memberType, bool allowUnmatchedMembers, MappingTemplateEnum mappingTemplate, List<string> excludeMemberNames )
        {
            Type templateType;
            if( mappingTemplate == MappingTemplateEnum.Source )
            {
                templateType = source.GetType();
            }
            else
            {
                templateType = target.GetType();
            }
            MemberInfo[] sourceMembers = _getTemplateMembers( source.GetType(), memberType );
            MemberInfo[] targetMembers = _getTemplateMembers( target.GetType(), memberType );
            Dictionary<string, MemberInfo> sourceMembersDict = _convertMemberInfoArrayToDictionary( sourceMembers, source.GetType() );
            Dictionary<string, MemberInfo> targetMembersDict = _convertMemberInfoArrayToDictionary( targetMembers, target.GetType() );
            Dictionary<string, MemberInfo> templateMembers = null;
            
            if( mappingTemplate == MappingTemplateEnum.Source )
            {
                templateMembers = sourceMembersDict;
            }
            else
            {
                templateMembers = targetMembersDict;
            }
            foreach( string templateMemberName in templateMembers.Keys )
            {
                if( excludeMemberNames == null || !excludeMemberNames.Contains( templateMemberName ) )
                {
                    MemberInfo sourceMember = null;
                    MemberInfo targetMember = null;

                    if( mappingTemplate == MappingTemplateEnum.Source )
                    {
                        if( !targetMembersDict.ContainsKey( templateMemberName ) )
                        {
                            if( !allowUnmatchedMembers )
                            {
                                throw new MissingTargetMemberException( templateMemberName );
                            }

                            continue;
                        }
                        targetMember = targetMembersDict[ templateMemberName ];
                        sourceMember = sourceMembersDict[ templateMemberName ];
                    }
                    else
                    {

                        if( !sourceMembersDict.ContainsKey( templateMemberName ) )
                        {
                            if( !allowUnmatchedMembers )
                            {
                                throw new MissingSourceMemberException( templateMemberName );
                            }
                            continue;
                        }

                        targetMember = targetMembersDict[ templateMemberName ];
                        sourceMember = sourceMembersDict[ templateMemberName ];
                    }
                    if( memberType == MemberTypeEnum.Property )
                    {
                        if( ( memberType == MemberTypeEnum.Property
                            && ( (PropertyInfo)sourceMember ).PropertyType != ( (PropertyInfo)targetMember ).PropertyType ) )
                        {

                            throw new MemberDataTypeMissmatchException( templateMemberName );
                        }

                        try
                        {
                            if( ( (PropertyInfo)sourceMember ).CanRead == true && ( (PropertyInfo)targetMember ).CanWrite == true )
                            {
                                ( (PropertyInfo)targetMember ).SetValue( target, ( (PropertyInfo)sourceMember ).GetValue( source, null ), null );
                            }

                        }
                        catch( Exception )
                        {
                            throw new MissingSourceMemberException( templateMemberName );
                        }

                    }
                    else if( memberType == MemberTypeEnum.Field )
                    {
                        if( ( memberType == MemberTypeEnum.Field && ( (FieldInfo)sourceMember ).FieldType != ( (FieldInfo)targetMember ).FieldType ) )
                        {

                            throw new MemberDataTypeMissmatchException( templateMemberName );
                        }

                        try
                        {


                            ( (FieldInfo)targetMember ).SetValue( target, ( (FieldInfo)sourceMember ).GetValue( source ) );
                        }
                        catch( Exception )
                        {
                            throw new MissingSourceMemberException( templateMemberName );
                        }
                    }
                }
            }
            return target;
        }

        private static MemberInfo[] _getTemplateMembers( Type templateType, MemberTypeEnum memberType )
        {
            MemberInfo[] templateMembers;
            if( memberType == MemberTypeEnum.Field )
            {
                templateMembers = templateType.GetFields( BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy );
            }
            else
            {
                templateMembers = templateType.GetProperties( BindingFlags.Public | BindingFlags.Instance );
            }
            return templateMembers;
        }

        private static MemberInfo _getMember( Type type, MemberInfo templateMember, MemberTypeEnum memberType )
        {
            MemberInfo instanceMember;
            if( memberType == MemberTypeEnum.Field )
            {
                instanceMember = type.GetField( templateMember.Name, BindingFlags.NonPublic | BindingFlags.Instance );
            }
            else
            {
                instanceMember = type.GetProperty( templateMember.Name );
            }
            return instanceMember;
        }

        private static MemberInfo _getMemberFromBase( Type baseType, string memberName, MemberTypeEnum memberType )
        {
            MemberInfo member;
            if( memberType == MemberTypeEnum.Field )
            {
                member = baseType.GetField( memberName, BindingFlags.NonPublic | BindingFlags.Instance );
            }
            else
            {
                member = baseType.GetProperty( memberName );
            }

            if( member == null )
            {
                if( baseType == typeof( System.Object ) )
                {
                    return null;
                }
                else
                {
                    member = _getMemberFromBase( baseType.BaseType, memberName, memberType );
                }
            }
            return member;
        }

        
        private static Dictionary<string, MemberInfo> _convertMemberInfoArrayToDictionary( MemberInfo[] arrayToConvert, Type objectReflectedOn )
        {
            //NOTE: Prefering declared members to inherited.
            Dictionary<string, MemberInfo> dictionaryToReturn = new Dictionary<string, MemberInfo>( arrayToConvert.Length );
            foreach( MemberInfo memberInfo in arrayToConvert )
            {
                if( memberInfo.DeclaringType == objectReflectedOn )
                {
                    dictionaryToReturn.Add( memberInfo.Name, memberInfo );
                }

            }
            foreach( MemberInfo memberInfo in arrayToConvert )
            {
                if( (!(memberInfo.DeclaringType == objectReflectedOn) && !dictionaryToReturn.ContainsKey( memberInfo.Name )) )
                {
                    dictionaryToReturn.Add( memberInfo.Name, memberInfo );

                }
            }

            return dictionaryToReturn;
        }

        private MemberMapper()
        {
        }
    }
}


