//-----------------------------------------------------------------------------
// Copyright (c) 2013 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

#include "platform/platform.h"
#include "sim/simBase.h"
#include "string/stringTable.h"
#include "console/console.h"
#include "io/fileStream.h"
#include "input/actionMap.h"
#include "io/resource/resourceManager.h"
#include "io/fileObject.h"
#include "console/consoleInternal.h"
#include "debug/profiler.h"
#include "console/consoleTypeValidators.h"
#include "memory/frameAllocator.h"

//////////////////////////////////////////////////////////////////////////
// Sim Set
//////////////////////////////////////////////////////////////////////////

void SimSet::addObject(SimObject* obj)
{
   lock();
   objectList.pushBack(obj);
   deleteNotify(obj);
   unlock();
}

void SimSet::removeObject(SimObject* obj)
{
   lock();
   objectList.remove(obj);
   clearNotify(obj);
   unlock();
}

void SimSet::pushObject(SimObject* pObj)
{
   lock();
   objectList.pushBackForce(pObj);
   deleteNotify(pObj);
   unlock();
}

void SimSet::popObject()
{
   MutexHandle handle;
   handle.lock(mMutex);

   if (objectList.size() == 0) 
   {
      AssertWarn(false, "Stack underflow in SimSet::popObject");
      return;
   }

   SimObject* pObject = objectList[objectList.size() - 1];

   objectList.removeStable(pObject);
   clearNotify(pObject);
}

//-----------------------------------------------------------------------------

void SimSet::callOnChildren( const char * method, S32 argc, const char *argv[], bool executeOnChildGroups )
{
   // Prep the arguments for the console exec...
   // Make sure and leave args[1] empty.
   const char* args[21];
   args[0] = method;
   for (S32 i = 0; i < argc; i++)
      args[i + 2] = argv[i];

   for( iterator i = begin(); i != end(); i++ )
   {
      SimObject *childObj = static_cast<SimObject*>(*i);

      if( childObj->isMethod( method ) )
         Con::execute(childObj, argc + 2, args);

      if( executeOnChildGroups )
      {
         SimSet* childSet = dynamic_cast<SimSet*>(*i);
         if ( childSet )
            childSet->callOnChildren( method, argc, argv, executeOnChildGroups );
      }
   }
}

bool SimSet::reOrder( SimObject *obj, SimObject *target )
{
   MutexHandle handle;
   handle.lock(mMutex);

   iterator itrS, itrD;
   if ( (itrS = find(begin(),end(),obj)) == end() )
   {
      return false;  // object must be in list
   }

   if ( obj == target )
   {
      return true;   // don't reorder same object but don't indicate error
   }

   if ( !target )    // if no target, then put to back of list
   {
      if ( itrS != (end()-1) )      // don't move if already last object
      {
         objectList.erase(itrS);    // remove object from its current location
         objectList.push_back(obj); // push it to the back of the list
      }
   }
   else              // if target, insert object in front of target
   {
      if ( (itrD = find(begin(),end(),target)) == end() )
         return false;              // target must be in list

      objectList.erase(itrS);

      //Tinman - once itrS has been erased, itrD won't be pointing at the same place anymore - re-find...
      itrD = find(begin(),end(),target);
      objectList.insert(itrD,obj);
   }

   return true;
}   

void SimSet::onDeleteNotify(SimObject *object)
{
   removeObject(object);
   Parent::onDeleteNotify(object);
}

void SimSet::onRemove()
{
   MutexHandle handle;
   handle.lock(mMutex);

   objectList.sortId();
   if (objectList.size())
   {
      // This backwards iterator loop doesn't work if the
      // list is empty, check the size first.
      for (SimObjectList::iterator ptr = objectList.end() - 1;
         ptr >= objectList.begin(); ptr--)
      {
         clearNotify(*ptr);
      }
   }

   handle.unlock();

   Parent::onRemove();
}

void SimSet::write(Stream &stream, U32 tabStop, U32 flags)
{
   MutexHandle handle;
   handle.lock(mMutex);

   // export selected only?
   if((flags & SelectedOnly) && !isSelected())
   {
      for(U32 i = 0; i < (U32)size(); i++)
         (*this)[i]->write(stream, tabStop, flags);

      return;

   }

   stream.writeTabs(tabStop);
   char buffer[1024];
   dSprintf(buffer, sizeof(buffer), "new %s(%s) {\r\n", getClassName(), getName() ? getName() : "");
   stream.write(dStrlen(buffer), buffer);
   writeFields(stream, tabStop + 1);

   if(size())
   {
      stream.write(2, "\r\n");
      for(U32 i = 0; i < (U32)size(); i++)
         (*this)[i]->write(stream, tabStop + 1, flags);
   }

   stream.writeTabs(tabStop);
   stream.write(4, "};\r\n");
}

void SimSet::deleteObjects( void )
{
    lock();
        while(size() > 0 )
        {
            objectList[0]->deleteObject();
        }
    unlock();
}

void SimSet::clear()
{
   lock();
   while (size() > 0)
      removeObject(objectList.last());
   unlock();
}

SimObject* SimSet::findObjectByInternalName(const char* internalName, bool searchChildren)
{
   iterator i;
   for (i = begin(); i != end(); i++)
   {
      SimObject *childObj = static_cast<SimObject*>(*i);
      if(childObj->getInternalName() == internalName)
         return childObj;
      else if (searchChildren)
      {
         SimSet* childSet = dynamic_cast<SimSet*>(*i);
         if (childSet)
         {
            SimObject* found = childSet->findObjectByInternalName(internalName, searchChildren);
            if (found) return found;
         }
      }
   }

   return NULL;
}

//////////////////////////////////////////////////////////////////////////

IMPLEMENT_CONOBJECT_CHILDREN(SimSet);


inline void SimSetIterator::Stack::push_back(SimSet* set)
{
   increment();
   last().set = set;
   last().itr = set->begin();
}


//-----------------------------------------------------------------------------

SimSetIterator::SimSetIterator(SimSet* set)
{
   VECTOR_SET_ASSOCIATION(stack);

   if (!set->empty())
      stack.push_back(set);
}


//-----------------------------------------------------------------------------

SimObject* SimSetIterator::operator++()
{
   SimSet* set;
   if ((set = dynamic_cast<SimSet*>(*stack.last().itr)) != 0) 
   {
      if (!set->empty()) 
      {
         stack.push_back(set);
         return *stack.last().itr;
      }
   }

   while (++stack.last().itr == stack.last().set->end()) 
   {
      stack.pop_back();
      if (stack.empty())
         return 0;
   }
   return *stack.last().itr;
}	

SimObject* SimGroupIterator::operator++()
{
   SimGroup* set;
   if ((set = dynamic_cast<SimGroup*>(*stack.last().itr)) != 0) 
   {
      if (!set->empty()) 
      {
         stack.push_back(set);
         return *stack.last().itr;
      }
   }

   while (++stack.last().itr == stack.last().set->end()) 
   {
      stack.pop_back();
      if (stack.empty())
         return 0;
   }
   return *stack.last().itr;
}	


//////////////////////////////////////////////////////////////////////////
// SimGroup
//////////////////////////////////////////////////////////////////////////

SimGroup::~SimGroup()
{
   lock();
   for (iterator itr = begin(); itr != end(); itr++)
      nameDictionary.remove(*itr);

   // XXX Move this later into Group Class
   // If we have any objects at this point, they should
   // already have been removed from the manager, so we
   // can just delete them directly.
   objectList.sortId();
   while (!objectList.empty()) 
   {
      delete objectList.last();
      objectList.decrement();
   }

   unlock();
}


//////////////////////////////////////////////////////////////////////////

void SimGroup::addObject(SimObject* obj)
{
   lock();

   // Make sure we aren't adding ourself.  This isn't the most robust check
   // but it should be good enough to prevent some self-foot-shooting.
   if(obj == this)
   {
      Con::errorf("SimGroup::addObject - (%d) can't add self!", getIdString());
      unlock();
      return;
   }

   if (obj->mGroup != this) 
   {
      if (obj->mGroup)
         obj->mGroup->removeObject(obj);
      nameDictionary.insert(obj);
      obj->mGroup = this;
      objectList.push_back(obj); // force it into the object list
      // doesn't get a delete notify
      obj->onGroupAdd();
   }
   unlock();
}

void SimGroup::removeObject(SimObject* obj)
{
   lock();
   if (obj->mGroup == this) 
   {
      obj->onGroupRemove();
      nameDictionary.remove(obj);
      objectList.remove(obj);
      obj->mGroup = 0;
   }
   unlock();
}

//////////////////////////////////////////////////////////////////////////

void SimGroup::onRemove()
{
   lock();
   objectList.sortId();
   if (objectList.size())
   {
      // This backwards iterator loop doesn't work if the
      // list is empty, check the size first.
      for (SimObjectList::iterator ptr = objectList.end() - 1;
         ptr >= objectList.begin(); ptr--)
      {
          if ( (*ptr)->isProperlyAdded() )
          {
             (*ptr)->onGroupRemove();
             (*ptr)->mGroup = NULL;
             (*ptr)->unregisterObject();
             (*ptr)->mGroup = this;
          }
      }
   }
   SimObject::onRemove();
   unlock();
}

//////////////////////////////////////////////////////////////////////////

SimObject *SimGroup::findObject(const char *namePath)
{
   // find the end of the object name
   S32 len;
   for(len = 0; namePath[len] != 0 && namePath[len] != '/'; len++)
      ;

   StringTableEntry stName = StringTable->lookupn(namePath, len);
   if(!stName)
      return NULL;

   SimObject *root = nameDictionary.find(stName);

   if(!root)
      return NULL;

   if(namePath[len] == 0)
      return root;

   return root->findObject(namePath + len + 1);
}

SimObject *SimSet::findObject(const char *namePath)
{
   // find the end of the object name
   S32 len;
   for(len = 0; namePath[len] != 0 && namePath[len] != '/'; len++)
      ;

   StringTableEntry stName = StringTable->lookupn(namePath, len);
   if(!stName)
      return NULL;

   lock();
   for(SimSet::iterator i = begin(); i != end(); i++)
   {
      if((*i)->getName() == stName)
      {
         unlock();
         if(namePath[len] == 0)
            return *i;
         return (*i)->findObject(namePath + len + 1);
      }
   }
   unlock();
   return NULL;
}

SimObject* SimObject::findObject(const char* )
{
   return NULL;
}

//////////////////////////////////////////////////////////////////////////

bool SimGroup::processArguments(S32, const char **)
{
   return true;
}

//////////////////////////////////////////////////////////////////////////

IMPLEMENT_CONOBJECT(SimGroup);

//////////////////////////////////////////////////////////////////////////
// Console Methods
//////////////////////////////////////////////////////////////////////////

ConsoleMethod(SimSet, listObjects, void, 2, 2, "() Prints the object data within the set\n"
              "@return No return value")
{
   object->lock();
   SimSet::iterator itr;
   for(itr = object->begin(); itr != object->end(); itr++)
   {
      SimObject *obj = *itr;
      bool isSet = dynamic_cast<SimSet *>(obj) != 0;
      const char *name = obj->getName();
      if(name)
         Con::printf("   %d,\"%s\": %s %s", obj->getId(), name,
         obj->getClassName(), isSet ? "(g)":"");
      else
         Con::printf("   %d: %s %s", obj->getId(), obj->getClassName(),
         isSet ? "(g)" : "");
   }
   object->unlock();
}

ConsoleMethod(SimSet, add, void, 3, 0, "(obj1,...) Adds given listy of objects to the SimSet.\n"
              "@param obj_i (i is unilimited) Variable list of objects to add\n"
              "@return No return value")
{
   for(S32 i = 2; i < argc; i++)
   {
      SimObject *obj = Sim::findObject(argv[i]);
      if(obj)
         object->addObject(obj);
      else
         Con::printf("Set::add: Object \"%s\" doesn't exist", argv[i]);
   }
}

ConsoleMethod(SimSet, remove, void, 3, 0, "(obj1,...) Removes given listy of objects from the SimSet.\n"
              "@param obj_i (i is unilimited) Variable list of objects to remove\n"
              "@return No return value")
{
   for(S32 i = 2; i < argc; i++)
   {
      SimObject *obj = Sim::findObject(argv[i]);
      object->lock();
      if(obj && object->find(object->begin(),object->end(),obj) != object->end())
         object->removeObject(obj);
      else
         Con::printf("Set::remove: Object \"%s\" does not exist in set", argv[i]);
      object->unlock();
   }
}

//-----------------------------------------------------------------------------

ConsoleMethod(SimSet, deleteObjects, void, 2, 2,    "() Deletes all the objects in the SimSet.\n"
                                                    "@return No return value")
{
    object->deleteObjects();
}

//-----------------------------------------------------------------------------

ConsoleMethod(SimSet, clear, void, 2, 2, "() Clears the Simset\n"
              "@return No return value")
{
   object->clear();
}

//-----------------------------------------------------------------------------

ConsoleMethod( SimSet, callOnChildren, void, 3, 0,
   "( string method, string args... ) Call a method on all objects contained in the set.\n\n"
   "@param method The name of the method to call.\n"
   "@param args The arguments to the method.\n\n"
   "@note This method recurses into all SimSets that are children to the set.\n\n"
   "@see callOnChildrenNoRecurse" )
{
   object->callOnChildren( argv[2], argc - 3, argv + 3 );
}

//////////////////////////////////////////////////////////////////////////-
//	Make Sure Child 1 is Ordered Just Under Child 2.
//////////////////////////////////////////////////////////////////////////-
ConsoleMethod(SimSet, reorderChild, void, 4,4," (child1, child2) Uses simset reorder to push child 1 before child 2 - both must already be child controls of this control\n"
              "@param child1 The child you wish to set first\n"
              "@param child2 The child you wish to set after child1\n"
              "@return No return value.")
{
   SimObject* pObject = Sim::findObject(argv[2]);
   SimObject* pTarget	 = Sim::findObject(argv[3]);

   if(pObject && pTarget)
   {
      object->reOrder(pObject,pTarget);
   }
}

ConsoleMethod(SimSet, getCount, S32, 2, 2, "() @return Returns the number of objects in the SimSet")
{
   return object->size();
}

ConsoleMethod(SimSet, getObject, S32, 3, 3, "(objIndex) @return Returns the ID of the desired object or -1 on failure")
{
   S32 objectIndex = dAtoi(argv[2]);
   if(objectIndex < 0 || objectIndex >= S32(object->size()))
   {
      Con::printf("Set::getObject index out of range.");
      return -1;
   }
   return ((*object)[objectIndex])->getId();
}

ConsoleMethod(SimSet, isMember, bool, 3, 3, "(object) @return Returns true if specified object is a member of the set, and false otherwise")
{
   SimObject *testObject = Sim::findObject(argv[2]);
   if(!testObject)
   {
      Con::printf("SimSet::isMember: %s is not an object.", argv[2]);
      return false;
   }

   object->lock();
   for(SimSet::iterator i = object->begin(); i != object->end(); i++)
   {
      if(*i == testObject)
      {
         object->unlock();
         return true;
      }
   }
   object->unlock();

   return false;
}

ConsoleMethod( SimSet, findObjectByInternalName, S32, 3, 4, "(string name, [bool searchChildren]) Returns the object with given internal name\n"
              "@param name The internal name of the object you wish to find\n"
              "@param searchChildren Set this true if you wish to search all children as well.\n"
              "@return Returns the ID of the object.")
{

   StringTableEntry pcName = StringTable->insert(argv[2]);
   bool searchChildren = false;
   if (argc > 3)
      searchChildren = dAtob(argv[3]);

   SimObject* child = object->findObjectByInternalName(pcName, searchChildren);
   if(child)
      return child->getId();
   return 0;
}

ConsoleMethod(SimSet, bringToFront, void, 3, 3, "(object) Brings object to front of set.\n"
              "@return No return value.")
{
   SimObject *obj = Sim::findObject(argv[2]);
   if(!obj)
      return;
   object->bringObjectToFront(obj);
}

ConsoleMethod(SimSet, pushToBack, void, 3, 3, "(object) Sends item to back of set.\n"
              "@return No return value.")
{
   SimObject *obj = Sim::findObject(argv[2]);
   if(!obj)
      return;
   object->pushObjectToBack(obj);
}
